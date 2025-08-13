using IAMS.Common;
using IAMS.Models.DeviceInfo;
using IAMS.Models.PowerStation;
using IAMS.Models.PriceTemplate;
using IAMS.MQTT;
using IAMS.MQTT.Model;
using IAMS.ViewModels.PowerStationOverview;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Org.BouncyCastle.Ocsp;

namespace IAMS.Service {
    public class PowerStationOverviewService : IPowerStationOverviewService {
        private IPowerStationService _powerStationService;
        private IStationSystemService _stationSystemService;
        private ITemplateService _templateService;
        private IClickHouseService _clickHouseService;
        private readonly ILogger<PowerStationOverviewService> _logger;
        public PowerStationOverviewService(ILogger<PowerStationOverviewService> logger, IPowerStationService powerStationService,
                IStationSystemService stationSystemService, ITemplateService templateService, IClickHouseService clickHouseService) {
            _logger = logger;
            _powerStationService = powerStationService;
            _stationSystemService = stationSystemService;
            _templateService = templateService;
            _clickHouseService = clickHouseService;

        }

        public List<SeriesData> GetChargeAdnDischargeChartData(int powerStationId, DateTime startDate = default, DateTime endDate = default) {
            List<SeriesData> seriesDatas = new List<SeriesData>();
            if (startDate == default || endDate == default) {
                startDate = DateTime.Now.AddDays(-7);
                endDate = DateTime.Now;
            }
            PowerStationInfo selectedPs = _powerStationService.GetAllPowerStationInfos(powerStationId, null).FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0).First(s => s.Id == powerStationId);

            var deviceInfos = _powerStationService.GetDeviceBaseInfosByPowerStationId(new List<int>() { selectedPs.Id });

            Dictionary<DateTime, double> date2charge = new Dictionary<DateTime, double>();
            Dictionary<DateTime, double> date2discharge = new Dictionary<DateTime, double>();
            List<PcsModel005> pcsInfos = _clickHouseService.GetPcsModel005s(deviceInfos.Where(s => s.DeviceType == (int)DeviceCode.PCS && s.PowerStationId == selectedPs.Id).Select(s => s.Sn).ToList(), startDate, endDate)
                .GroupBy(m => new { m.Sn, Date = m.UploadTime.Date }).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();
            for (DateTime current = startDate; current < endDate; current = current.AddDays(1)) {
                double charge = 0;
                double discharge = 0;
                List<PcsModel005> currentPcsInfo = pcsInfos.FindAll(s => s.UploadTime.Date == current.Date);
                if (currentPcsInfo != null) {
                    charge = currentPcsInfo.Sum(s => s.DailyChargeAmount);
                    discharge = currentPcsInfo.Sum(s => s.DailyDischargeAmount);
                }
                date2charge[current] = date2charge.GetValueOrDefault(current);
                date2discharge[current] = date2discharge.GetValueOrDefault(current);
            }


            seriesDatas.Add(new SeriesData() {
                Name = "充电功率",
                Data = date2charge.OrderBy(kv => kv.Key).Select(kv => new object[] { kv.Key, kv.Value }).ToList()
            });
            seriesDatas.Add(new SeriesData() {
                Name = "放电功率",
                Data = date2discharge.OrderBy(kv => kv.Key).Select(kv => new object[] { kv.Key, kv.Value }).ToList()
            });

            return seriesDatas;

        }

        public SeriesData GetEarnSummaryData(int powerStationId, DateTime startDate = default, DateTime endDate = default) {
            SeriesData seriesData = new SeriesData() {
                Name = "收益统计"
            };
            if (startDate == default || endDate == default) {
                startDate = DateTime.Now.AddDays(-7);
                endDate = DateTime.Now;
            }

            PowerStationInfo selectedPs = _powerStationService.GetAllPowerStationInfos(powerStationId, null).FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0).First(s => s.Id == powerStationId);

            var deviceInfos = _powerStationService.GetDeviceBaseInfosByPowerStationId(new List<int>() { selectedPs.Id });
            PriceTemplateInfo templateInfo = _templateService.GetTemplateByPowerStationId(selectedPs.Id);
            Dictionary<DateTime, decimal> date2earn = new Dictionary<DateTime, decimal>();
            List<PccModel001> pcsInfos = _clickHouseService.GetPccModel001s(deviceInfos.Where(s => s.DeviceType == (int)DeviceCode.PCC && s.PowerStationId == selectedPs.Id).Select(s => s.Sn).ToList(), startDate, endDate)
                .GroupBy(m => new { m.Sn, Date = m.UploadTime.Date }).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();

            for (DateTime current = startDate; current < endDate; current = current.AddDays(1)) {

                List<PccModel001> currentGatewayTableModelInfo = pcsInfos.FindAll(s => s.UploadTime.Date == current.Date);
                decimal earn = _templateService.GetEarn(currentGatewayTableModelInfo, templateInfo);
                if (date2earn.ContainsKey(current)) {
                    date2earn[current] += earn;
                } else {
                    date2earn[current] = earn;
                }
            }

            seriesData.Data = date2earn.OrderBy(kv => kv.Key).Select(kv => new object[] { kv.Key, kv.Value }).ToList();
            return seriesData;
        }

        public PowerStationOverviewViewModel GetPowerStationOverviewViewModel(int id) {
            PowerStationOverviewViewModel ret = new PowerStationOverviewViewModel();
            try {
                ret.PowerStationInfos = _powerStationService.GetAllPowerStationInfos(id, null).FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0);
                if (ret.PowerStationInfos.Count == 0) {
                    throw new Exception("No Data");
                }

                ret.SelectedPowerStation = ret.PowerStationInfos.FirstOrDefault(s => s.IsSelected);
                if (ret.SelectedPowerStation == null) {
                    return ret;
                }

                var deviceInfos = _powerStationService.GetDeviceBaseInfosByPowerStationId(new List<int>() { ret.SelectedPowerStation.Id });

                List<PcsModel005> pcsInfos_today = _clickHouseService.GetPcsModel005s(deviceInfos.Where(s => s.DeviceType == (int)DeviceCode.PCS && s.PowerStationId == ret.SelectedPowerStation.Id).Select(s => s.Sn).ToList(), DateTime.Today).GroupBy(s => s.Sn).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();
                List<PcsModel005> pcsInfos_yesterday = _clickHouseService.GetPcsModel005s(deviceInfos.Where(s => s.DeviceType == (int)DeviceCode.PCS && s.PowerStationId == ret.SelectedPowerStation.Id).Select(s => s.Sn).ToList(), DateTime.Today.AddDays(-1)).GroupBy(s => s.Sn).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();
                var charge_today = pcsInfos_today.Sum(s => s.DailyChargeAmount);
                ret.ChargeAmountToday += charge_today;
                ret.ChargeAmountRaise += charge_today - pcsInfos_yesterday.Sum(s => s.DailyChargeAmount);
                ret.DischargeAmountToday += pcsInfos_today.Sum(s => s.DailyDischargeAmount);
                ret.DischargeAmountRaise += pcsInfos_today.Sum(s => s.DailyDischargeAmount) - pcsInfos_yesterday.Sum(s => s.DailyDischargeAmount);

                //根据ps查到绑定的电价模板
                PriceTemplateInfo templateInfo = _templateService.GetTemplateByPowerStationId(ret.SelectedPowerStation.Id);
                List<PccModel001> gatewayTableModelInfos = _clickHouseService.GetPccModel001s(deviceInfos.Where(s => s.DeviceType == (int)DeviceCode.PCC).Select(s => s.Sn).ToList(), DateTime.Today)
                    .GroupBy(s => s.Sn).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();
                ret.EarningToday += _templateService.GetEarn(gatewayTableModelInfos, templateInfo);
                List<PccModel001> gatewayTableModelInfos_yesterday = _clickHouseService.GetPccModel001s(deviceInfos.Where(s => s.DeviceType == (int)DeviceCode.PCC).Select(s => s.Sn).ToList(), DateTime.Today.AddDays(-1))
                    .GroupBy(s => s.Sn).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();
                ret.EarningRaise += ret.EarningToday - _templateService.GetEarn(gatewayTableModelInfos_yesterday, templateInfo);

                //获取当月每天的所有的GatewayTableModelInfo
                List<PccModel001> gatewayTableModelInfo_month = _clickHouseService.GetPccModel001s(deviceInfos.Where(s => s.DeviceType == (int)DeviceCode.PCC).Select(s => s.Sn).ToList(), Utility.GetStartOfMonth(DateTime.Now), Utility.GetEndOfMonth(DateTime.Now))
                    .GroupBy(m => new { m.Sn, Date = m.UploadTime.Date }).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();
                ret.EarningThisMonth += _templateService.GetEarn(gatewayTableModelInfo_month, templateInfo);
                //获取历史的所有的GatewayTableModelInfo
                List<PccModel001> gatewayTableModelInfo_total = _clickHouseService.GetPccModel001s(deviceInfos.Where(s => s.DeviceType == (int)DeviceCode.PCC).Select(s => s.Sn).ToList(), Utility.GetMinDate(), Utility.GetMaxDate())
                    .GroupBy(m => new { m.Sn, Date = m.UploadTime.Date }).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();
                ret.TotalEarning += _templateService.GetEarn(gatewayTableModelInfo_total, templateInfo);

                foreach (EnergyStorageCabinetInfo cabinet in ret.SelectedPowerStation.EnergyStorageCabinetRootDataList) {
                    //堆中每个系统的详情
                    ret.CabinetStationSystemInfos.Add(new CabinetStationSystemInfo() {
                        CabinetSn = cabinet.CabinetSn,
                        OnlineStatus = "在线",//后期要改structure状态
                        DailyDischargeAmount = charge_today
                    });



                }

            } catch (Exception ex) {
                _logger.LogError(ex.ToString());
                throw;
            }
            return ret;
        }

    }
}
