using IAMS.Common;
using IAMS.Models.PowerStation;
using IAMS.Models.PriceTemplate;
using IAMS.Models.StationSystem;
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
        private readonly ILogger<PowerStationOverviewService> _logger;
        public PowerStationOverviewService(ILogger<PowerStationOverviewService> logger,IPowerStationService powerStationService, IStationSystemService stationSystemService, ITemplateService templateService) {
            _logger = logger;
            _powerStationService = powerStationService;
            _stationSystemService = stationSystemService;
            _templateService = templateService;
            
        }

        public List<SeriesData> GetChargeAdnDischargeChartData(int powerStationId, DateTime startDate = default, DateTime endDate = default) {
            List<SeriesData> seriesDatas = new List<SeriesData>();
            if (startDate == default || endDate == default) {
                startDate = DateTime.Now.AddDays(-7);
                endDate = DateTime.Now;
            }
            PowerStationInfo selectedPs = _powerStationService.GetAllPowerStationInfos().FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0).First(s => s.Id == powerStationId);
            Dictionary<DateTime, double> date2charge = new Dictionary<DateTime, double>();
            Dictionary<DateTime, double> date2discharge = new Dictionary<DateTime, double>();
            foreach (EnergyStorageCabinetInfo energyStorageCabinetInfo in selectedPs.EnergyStorageCabinetRootDataList) {
                List<Structure> pcsRootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(energyStorageCabinetInfo.rootDataFromMqtt.structure, (int)DeviceCode.PCS, 1);
                List<PCSInfo> pcsInfos = _stationSystemService.GetPCSInfo(pcsRootInfos.Select(S => S.name).ToList(), startDate, endDate)
                    .GroupBy(m => new { m.DevName, Date = m.UploadTime.Date }).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();
                for (DateTime current = startDate; current < endDate; current = current.AddDays(1)) {
                    double charge = 0;
                    double discharge = 0;
                    List<PCSInfo> currentPcsInfo = pcsInfos.FindAll(s => s.UploadTime.Date == current.Date);
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

        public SeriesData GetEarnSummaryData(int powerStationId, DateTime startDate = default, DateTime endDate = default) {
            SeriesData seriesData = new SeriesData() {
                Name = "收益统计"
            };
            if (startDate == default || endDate == default) {
                startDate = DateTime.Now.AddDays(-7);
                endDate = DateTime.Now;
            }

            PowerStationInfo selectedPs = _powerStationService.GetAllPowerStationInfos().FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0).First(s => s.Id == powerStationId);
            PriceTemplateInfo templateInfo = _templateService.GetTemplateByPowerStationId(selectedPs.Id);
            Dictionary<DateTime, decimal> date2earn = new Dictionary<DateTime, decimal>();
            foreach (EnergyStorageCabinetInfo cabinet in selectedPs.EnergyStorageCabinetRootDataList) {
                List<Structure> gatewayTableRootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(cabinet.rootDataFromMqtt.structure, (int)DeviceCode.GatewayTableModel, 1);
                List<GatewayTableModelInfo> gatewayTableModelInfos = _stationSystemService.GetGatewayTableModelInfo(gatewayTableRootInfos.Select(S => S.name).ToList(), startDate, endDate)
                    .GroupBy(m => new { m.DevName, Date = m.UploadTime.Date }).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();

                for (DateTime current = startDate; current < endDate; current = current.AddDays(1)) {
                    
                    List<GatewayTableModelInfo> currentGatewayTableModelInfo = gatewayTableModelInfos.FindAll(s => s.UploadTime.Date == current.Date);
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

        public PowerStationOverviewViewModel GetPowerStationOverviewViewModel(int id) {
            PowerStationOverviewViewModel ret = new PowerStationOverviewViewModel();
            try {
                ret.PowerStationInfos = _powerStationService.GetAllPowerStationInfos().FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0);
                if (ret.PowerStationInfos.Count == 0) {
                    throw new Exception("No Data");
                }
                if (id == 0) {
                    ret.PowerStationInfos.First().IsSelected = true;
                } else {
                    ret.PowerStationInfos.First(s => s.Id == id).IsSelected = true;
                }

                ret.SelectedPowerStation = ret.PowerStationInfos.FirstOrDefault(s => s.IsSelected);
                if (ret.SelectedPowerStation == null) {
                    return ret;
                }

                foreach (EnergyStorageCabinetInfo cabinet in ret.SelectedPowerStation.EnergyStorageCabinetRootDataList) {
                    List<Structure> rootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(cabinet.rootDataFromMqtt.structure, (int)DeviceCode.PCS, 1);
                    List<PCSInfo> pcsInfos_today = _stationSystemService.GetPCSInfo(rootInfos.Select(S => S.name).ToList(), DateTime.Today).GroupBy(s => s.DevName).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();
                    List<PCSInfo> pcsInfos_yesterday = _stationSystemService.GetPCSInfo(rootInfos.Select(S => S.name).ToList(), DateTime.Today.AddDays(-1)).GroupBy(s => s.DevName).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();

                    var charge_today = pcsInfos_today.Sum(s => s.DailyChargeAmount);
                    ret.ChargeAmountToday += charge_today;
                    ret.ChargeAmountRaise += charge_today - pcsInfos_yesterday.Sum(s => s.DailyChargeAmount);
                    ret.DischargeAmountToday += pcsInfos_today.Sum(s => s.DailyDischargeAmount);
                    ret.DischargeAmountRaise += pcsInfos_today.Sum(s => s.DailyDischargeAmount) - pcsInfos_yesterday.Sum(s => s.DailyDischargeAmount);

                    rootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(cabinet.rootDataFromMqtt.structure, (int)DeviceCode.GatewayTableModel, 1);

                    //堆中每个系统的详情
                    ret.CabinetStationSystemInfos.Add(new CabinetStationSystemInfo() {
                        CabinetName = cabinet.rootDataFromMqtt.structure.name,
                        SystemName = cabinet.rootDataFromMqtt.structure.name,//后期改为主机从机
                        OnlineStatus = "在线",//后期要改structure状态
                        DailyDischargeAmount = charge_today
                    });

                    //根据ps查到绑定的电价模板
                    PriceTemplateInfo templateInfo = _templateService.GetTemplateByPowerStationId(ret.SelectedPowerStation.Id);
                    List<GatewayTableModelInfo> gatewayTableModelInfos = _stationSystemService.GetGatewayTableModelInfo(rootInfos.Select(S => S.name).ToList(), DateTime.Today)
                        .GroupBy(s => s.DevName).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();
                    ret.EarningToday += _templateService.GetEarn(gatewayTableModelInfos, templateInfo);
                    List<GatewayTableModelInfo> gatewayTableModelInfos_yesterday = _stationSystemService.GetGatewayTableModelInfo(rootInfos.Select(S => S.name).ToList(), DateTime.Today.AddDays(-1))
                        .GroupBy(s => s.DevName).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();
                    ret.EarningRaise += ret.EarningToday - _templateService.GetEarn(gatewayTableModelInfos_yesterday, templateInfo);

                    //获取当月每天的所有的GatewayTableModelInfo
                    List<GatewayTableModelInfo> gatewayTableModelInfo_month = _stationSystemService.GetGatewayTableModelInfo(rootInfos.Select(S => S.name).ToList(), Utility.GetStartOfMonth(DateTime.Now), Utility.GetEndOfMonth(DateTime.Now))
                        .GroupBy(m => new { m.DevName, Date = m.UploadTime.Date }).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();
                    ret.EarningThisMonth += _templateService.GetEarn(gatewayTableModelInfo_month, templateInfo);
                    //获取历史的所有的GatewayTableModelInfo
                    List<GatewayTableModelInfo> gatewayTableModelInfo_total = _stationSystemService.GetGatewayTableModelInfo(rootInfos.Select(S => S.name).ToList(), Utility.GetMinDate(), Utility.GetMaxDate())
                        .GroupBy(m => new { m.DevName, Date = m.UploadTime.Date }).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();
                    ret.TotalEarning += _templateService.GetEarn(gatewayTableModelInfo_total, templateInfo);

                }
                
            } catch (Exception ex) {
                _logger.LogError(ex.ToString());
                throw;
            }
            return ret;
        }

    }
}
