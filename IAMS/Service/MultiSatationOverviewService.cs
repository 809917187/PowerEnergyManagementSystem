using IAMS.Common;
using IAMS.Models.DeviceInfo;
using IAMS.Models.PowerStation;
using IAMS.Models.PriceTemplate;
using IAMS.MQTT;
using IAMS.MQTT.Model;
using IAMS.ViewModels.MultiStationOverview;

namespace IAMS.Service {
    public class MultiSatationOverviewService : IMultiSatationOverviewService {
        private IPowerStationService _powerStationService;
        private IStationSystemService _stationSystemService;
        private ITemplateService _templateService;
        private IClickHouseService _clickHouseService;
        public MultiSatationOverviewService(IPowerStationService powerStationService, IStationSystemService stationSystemService,
                ITemplateService templateService, IClickHouseService clickHouseService) {
            _powerStationService = powerStationService;
            _stationSystemService = stationSystemService;
            _templateService = templateService;
            _clickHouseService = clickHouseService;
        }

        public List<SeriesData> GetChargeAdnDischargeChartData(DateTime startDate = default, DateTime endDate = default) {
            List<SeriesData> seriesDatas = new List<SeriesData>();
            if (startDate == default || endDate == default) {
                startDate = DateTime.Now.AddDays(-7);
                endDate = DateTime.Now;
            }
            var allPs = _powerStationService.GetAllPowerStationInfos(0, null).FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0);

            Dictionary<DateTime, double> date2charge = new Dictionary<DateTime, double>();
            Dictionary<DateTime, double> date2discharge = new Dictionary<DateTime, double>();
            List<DeviceBaseInfo> deviceBaseInfos = _powerStationService.GetDeviceBaseInfosByPowerStationId(allPs.Select(s => s.Id).ToList());
            foreach (var ps in allPs) {

                List<PcsModel005> pcsInfos = _clickHouseService.GetPcsModel005s(deviceBaseInfos.Where(s => s.DeviceType == (int)DeviceCode.PCS && s.PowerStationId == ps.Id).Select(s => s.Sn).ToList(), startDate, endDate)
                    .GroupBy(m => new { m.Sn, Date = m.UploadTime.Date }).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();
                for (DateTime current = startDate; current < endDate; current = current.AddDays(1)) {
                    double charge = 0;
                    double discharge = 0;
                    List<PcsModel005> currentPcsInfo = pcsInfos.FindAll(s => s.UploadTime.Date == current.Date);
                    if (currentPcsInfo != null) {
                        charge = currentPcsInfo.Sum(s => s.DailyChargeAmount);
                        discharge = currentPcsInfo.Sum(s => s.DailyDischargeAmount);
                    }
                    date2charge[current] = date2charge.GetValueOrDefault(current) + charge;
                    date2discharge[current] = date2discharge.GetValueOrDefault(current) + discharge;
                }

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
        public SeriesData GetEarnRankData(DateTime startDate = default, DateTime endDate = default) {
            SeriesData seriesData = new SeriesData() {
                Name = "收益排名"
            };
            if (startDate == default || endDate == default) {
                startDate = DateTime.Now.AddDays(-7);
                endDate = DateTime.Now;
            }

            var allPs = _powerStationService.GetAllPowerStationInfos(0, null).FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0);
            Dictionary<string, decimal> date2earn = new Dictionary<string, decimal>();//电站--》收益
            var deviceInfos = _powerStationService.GetDeviceBaseInfosByPowerStationId(allPs.Select(s => s.Id).ToList());
            foreach (var ps in allPs) {
                decimal earn = 0;
                PriceTemplateInfo templateInfo = _templateService.GetTemplateByPowerStationId(ps.Id);
                List<PccModel001> gatewayTableModelInfos = _clickHouseService.GetPccModel001s(deviceInfos.Where(s => s.DeviceType == (int)DeviceCode.PCC).Select(S => S.Sn).ToList(), startDate, endDate)
                    .GroupBy(m => new { m.Sn, Date = m.UploadTime.Date }).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();

                earn += _templateService.GetEarn(gatewayTableModelInfos, templateInfo);
                seriesData.Data.Add(new object[] { ps.Name, earn });

            }
            return seriesData;

        }
        public SeriesData GetEarnSummaryData(DateTime startDate = default, DateTime endDate = default) {
            SeriesData seriesData = new SeriesData() {
                Name = "收益统计"
            };
            if (startDate == default || endDate == default) {
                startDate = DateTime.Now.AddDays(-7);
                endDate = DateTime.Now;
            }
            var allPs = _powerStationService.GetAllPowerStationInfos(0, null).FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0);
            Dictionary<DateTime, decimal> date2earn = new Dictionary<DateTime, decimal>();
            var deviceInfos = _powerStationService.GetDeviceBaseInfosByPowerStationId(allPs.Select(s => s.Id).ToList());
            foreach (var ps in allPs) {
                PriceTemplateInfo templateInfo = _templateService.GetTemplateByPowerStationId(ps.Id);
                List<PccModel001> gatewayTableModelInfos = _clickHouseService.GetPccModel001s(deviceInfos.Where(s => s.DeviceType == (int)DeviceCode.PCC).Select(S => S.Sn).ToList(), startDate, endDate);

                for (DateTime current = startDate; current < endDate; current = current.AddDays(1)) {

                    List<PccModel001> currentGatewayTableModelInfo = gatewayTableModelInfos.FindAll(s => s.UploadTime.Date == current.Date);
                    decimal earn = _templateService.GetEarn(currentGatewayTableModelInfo, templateInfo);
                    if (date2earn.ContainsKey(current)) {
                        date2earn[current] += earn;
                    } else {
                        date2earn[current] = earn;
                    }
                }

            }
            seriesData.Data = date2earn.OrderBy(kv => kv.Key).Select(kv => new object[] { kv.Key, kv.Value }).ToList();
            return seriesData;
        }

        public MultiStationOverviewViewModel GetMultiStationOverviewViewModel() {
            MultiStationOverviewViewModel ret = new MultiStationOverviewViewModel();

            var allPS = _powerStationService.GetAllPowerStationInfos(0, null);
            var allOnlinePS = allPS.FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0);
            var deviceInfos = _powerStationService.GetDeviceBaseInfosByPowerStationId(allPS.Select(s => s.Id).ToList());

            ret.OnlinePowerStation = allOnlinePS;
            ret.OfflinePowerStation = allPS.FindAll(s => s.EnergyStorageCabinetRootDataList.Count == 0);
            ret.PowerStationTotalNumber = allPS.Count;
            ret.PowerStationOnlineNumber = allOnlinePS.Count;
            ret.PowerStationOfflineNumber = allPS.Count - allOnlinePS.Count;

            foreach (var ps in allPS) {
                ret.InstalledPower += ps.InstalledPower;
                ret.InstalledCapacity += ps.InstalledCapacity;
            }


            foreach (PowerStationInfo ps in allOnlinePS) {
                List<PcsModel005> pcsInfos_month = _clickHouseService.GetPcsModel005s(deviceInfos.Where(s => s.DeviceType == (int)DeviceCode.PCS && s.PowerStationId == ps.Id).Select(s => s.Sn).ToList(), Utility.GetStartOfMonth(DateTime.Now), Utility.GetEndOfMonth(DateTime.Now));
                List<PcsModel005> pcsInfos_today = pcsInfos_month.Where(s => s.UploadTime.Date == DateTime.Today.Date).ToList();
                List<PcsModel005> pcsInfos_yesterday = pcsInfos_month.Where(s => s.UploadTime.Date == DateTime.Today.AddDays(-1).Date).ToList();

                var charge_today = pcsInfos_today.Sum(s => s.DailyChargeAmount);
                ret.ChargeAmountToday += charge_today;
                ret.ChargeAmountRaise += charge_today - pcsInfos_yesterday.Sum(s => s.DailyChargeAmount);
                ret.DischargeAmountToday += pcsInfos_today.Sum(s => s.DailyDischargeAmount);
                ret.DischargeAmountRaise += pcsInfos_today.Sum(s => s.DailyDischargeAmount) - pcsInfos_yesterday.Sum(s => s.DailyDischargeAmount);
                ret.ChargeAmountThisMonth += pcsInfos_month.Sum(s => s.DailyChargeAmount);
                ret.DischargeAmountThisMonth += pcsInfos_month.Sum(s => s.DailyDischargeAmount);
                ret.TotalChargeAmount += pcsInfos_today.Sum(s => s.ACAccumulatedChargeAmount);
                ret.TotalDischargeAmount += pcsInfos_today.Sum(s => s.ACAccumulatedDischargeAmount);

                //根据ps查到绑定的电价模板
                PriceTemplateInfo templateInfo = _templateService.GetTemplateByPowerStationId(ps.Id);
                List<PccModel001> gatewayTableModelInfo_total = _clickHouseService.GetPccModel001s(deviceInfos.Where(s => s.DeviceType == (int)DeviceCode.PCC && s.PowerStationId == ps.Id).Select(s => s.Sn).ToList(), Utility.GetMinDate(), Utility.GetMaxDate());
                List<PccModel001> gatewayTableModelInfo_month = _clickHouseService.GetPccModel001s(deviceInfos.Where(s => s.DeviceType == (int)DeviceCode.PCC && s.PowerStationId == ps.Id).Select(s => s.Sn).ToList(), Utility.GetStartOfMonth(DateTime.Now), Utility.GetEndOfMonth(DateTime.Now));
                List<PccModel001> gatewayTableModelInfos_today = gatewayTableModelInfo_month.Where(s => s.UploadTime.Date == DateTime.Today.Date).ToList();
                List<PccModel001> gatewayTableModelInfos_yesterday = gatewayTableModelInfo_month.Where(s => s.UploadTime.Date == DateTime.Today.AddDays(-1).Date).ToList();
                ret.EarningToday += _templateService.GetEarn(gatewayTableModelInfos_today, templateInfo);
                ret.EarningRaise += ret.EarningToday - _templateService.GetEarn(gatewayTableModelInfos_yesterday, templateInfo);
                //获取当月每天的所有的GatewayTableModelInfo
                ret.EarningThisMonth += _templateService.GetEarn(gatewayTableModelInfo_month, templateInfo);
                //获取历史的所有的GatewayTableModelInfo
                ret.TotalEarning += _templateService.GetEarn(gatewayTableModelInfo_total, templateInfo);
            }
            return ret;
        }
    }
}
