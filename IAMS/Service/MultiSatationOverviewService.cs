using IAMS.Common;
using IAMS.Models.PowerStation;
using IAMS.Models.PriceTemplate;
using IAMS.Models.StationSystem;
using IAMS.MQTT;
using IAMS.MQTT.Model;
using IAMS.ViewModels.MultiStationOverview;

namespace IAMS.Service {
    public class MultiSatationOverviewService : IMultiSatationOverviewService {
        private IPowerStationService _powerStationService;
        private IStationSystemService _stationSystemService;
        private ITemplateService _templateService;
        public MultiSatationOverviewService(IPowerStationService powerStationService, IStationSystemService stationSystemService, ITemplateService templateService) {
            _powerStationService = powerStationService;
            _stationSystemService = stationSystemService;
            _templateService = templateService;
        }

        public List<SeriesData> GetChargeAdnDischargeChartData(DateTime startDate = default, DateTime endDate = default) {
            List<SeriesData> seriesDatas = new List<SeriesData>();
            if (startDate == default || endDate == default) {
                startDate = DateTime.Now.AddDays(-7);
                endDate = DateTime.Now;
            }
            var allPs = _powerStationService.GetAllPowerStationInfos().FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0);

            Dictionary<DateTime, double> date2charge = new Dictionary<DateTime, double>();
            Dictionary<DateTime, double> date2discharge = new Dictionary<DateTime, double>();
            foreach (var ps in allPs) {
                foreach (EnergyStorageCabinetInfo energyStorageCabinetInfo in ps.EnergyStorageCabinetRootDataList) {
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

            var allPs = _powerStationService.GetAllPowerStationInfos().FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0);
            Dictionary<string, decimal> date2earn = new Dictionary<string, decimal>();//电站--》收益
            foreach (var ps in allPs) {
                decimal earn = 0;
                PriceTemplateInfo templateInfo = _templateService.GetTemplateByPowerStationId(ps.Id);
                foreach (EnergyStorageCabinetInfo cabinet in ps.EnergyStorageCabinetRootDataList) {
                    List<Structure> gatewayTableRootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(cabinet.rootDataFromMqtt.structure, (int)DeviceCode.GatewayTableModel, 1);
                    List<GatewayTableModelInfo> gatewayTableModelInfos = _stationSystemService.GetGatewayTableModelInfo(gatewayTableRootInfos.Select(S => S.name).ToList(), startDate, endDate)
                        .GroupBy(m => new { m.DevName, Date = m.UploadTime.Date }).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();

                    earn += _templateService.GetEarn(gatewayTableModelInfos, templateInfo);
                }
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
            var allPs = _powerStationService.GetAllPowerStationInfos().FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0);
            Dictionary<DateTime, decimal> date2earn = new Dictionary<DateTime, decimal>();
            foreach (var ps in allPs) {
                PriceTemplateInfo templateInfo = _templateService.GetTemplateByPowerStationId(ps.Id);
                foreach (EnergyStorageCabinetInfo cabinet in ps.EnergyStorageCabinetRootDataList) {
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

            }
            seriesData.Data = date2earn.OrderBy(kv => kv.Key).Select(kv => new object[] { kv.Key, kv.Value }).ToList();
            return seriesData;
        }

        public MultiStationOverviewViewModel GetMultiStationOverviewViewModel() {
            MultiStationOverviewViewModel ret = new MultiStationOverviewViewModel();

            var allPS = _powerStationService.GetAllPowerStationInfos();
            var allOnlinePS = allPS.FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0);

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
                foreach (EnergyStorageCabinetInfo cabinet in ps.EnergyStorageCabinetRootDataList) {
                    List<Structure> rootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(cabinet.rootDataFromMqtt.structure, (int)DeviceCode.PCS, 1);
                    List<PCSInfo> pcsInfos_total = _stationSystemService.GetPCSInfo(rootInfos.Select(S => S.name).ToList(), Utility.GetMinDate(), Utility.GetMaxDate())
                        .GroupBy(m => new { m.DevName, Date = m.UploadTime.Date }).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();
                    List<PCSInfo> pcsInfos_today = pcsInfos_total.Where(s => s.UploadTime.Date == DateTime.Today.Date).ToList();
                    List<PCSInfo> pcsInfos_yesterday = pcsInfos_total.Where(s => s.UploadTime.Date == DateTime.Today.AddDays(-1).Date).ToList();
                    List<PCSInfo> pcsInfos_month = pcsInfos_total.Where(s => s.UploadTime.Date >= Utility.GetStartOfMonth(DateTime.Now).Date && s.UploadTime.Date <= Utility.GetEndOfMonth(DateTime.Now).Date).ToList();

                    var charge_today = pcsInfos_today.Sum(s => s.DailyChargeAmount);
                    ret.ChargeAmountToday += charge_today;
                    ret.ChargeAmountRaise += charge_today - pcsInfos_yesterday.Sum(s => s.DailyChargeAmount);
                    ret.DischargeAmountToday += pcsInfos_today.Sum(s => s.DailyDischargeAmount);
                    ret.DischargeAmountRaise += pcsInfos_today.Sum(s => s.DailyDischargeAmount) - pcsInfos_yesterday.Sum(s => s.DailyDischargeAmount);
                    ret.ChargeAmountThisMonth += pcsInfos_month.Sum(s => s.DailyChargeAmount);
                    ret.DischargeAmountThisMonth += pcsInfos_month.Sum(s => s.DailyDischargeAmount);
                    ret.TotalChargeAmount += pcsInfos_total.Sum(s => s.DailyChargeAmount);
                    ret.TotalDischargeAmount += pcsInfos_total.Sum(s => s.DailyDischargeAmount);

                    //根据ps查到绑定的电价模板
                    PriceTemplateInfo templateInfo = _templateService.GetTemplateByPowerStationId(ps.Id);
                    rootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(cabinet.rootDataFromMqtt.structure, (int)DeviceCode.GatewayTableModel, 1);
                    List<GatewayTableModelInfo> gatewayTableModelInfo_total = _stationSystemService.GetGatewayTableModelInfo(rootInfos.Select(S => S.name).ToList(), Utility.GetMinDate(), Utility.GetMaxDate())
                        .GroupBy(m => new { m.DevName, Date = m.UploadTime.Date }).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();

                    List<GatewayTableModelInfo> gatewayTableModelInfos = gatewayTableModelInfo_total.Where(s => s.UploadTime.Date == DateTime.Today.Date).ToList();
                    List<GatewayTableModelInfo> gatewayTableModelInfos_yesterday = gatewayTableModelInfo_total.Where(s => s.UploadTime.Date == DateTime.Today.AddDays(-1).Date).ToList();
                    List<GatewayTableModelInfo> gatewayTableModelInfo_month = gatewayTableModelInfo_total.Where(s => s.UploadTime.Date >= Utility.GetStartOfMonth(DateTime.Now).Date && s.UploadTime.Date <= Utility.GetEndOfMonth(DateTime.Now).Date).ToList();
                    ret.EarningToday += _templateService.GetEarn(gatewayTableModelInfos, templateInfo);
                    ret.EarningRaise += ret.EarningToday - _templateService.GetEarn(gatewayTableModelInfos_yesterday, templateInfo);
                    //获取当月每天的所有的GatewayTableModelInfo
                    ret.EarningThisMonth += _templateService.GetEarn(gatewayTableModelInfo_month, templateInfo);
                    //获取历史的所有的GatewayTableModelInfo
                    ret.TotalEarning += _templateService.GetEarn(gatewayTableModelInfo_total, templateInfo);
                }
            }
            return ret;
        }
    }
}
