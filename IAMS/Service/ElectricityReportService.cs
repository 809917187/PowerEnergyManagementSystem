using IAMS.Common;
using IAMS.Models.PowerStation;
using IAMS.Models.StationSystem;
using IAMS.MQTT;
using IAMS.MQTT.Model;
using IAMS.ViewModels.ElectricityReport;

namespace IAMS.Service {
    public class ElectricityReportService : IElectricityReportService {
        private IPowerStationService _powerStationService;
        private IStationSystemService _stationSystemService;
        private ITemplateService _templateService;
        public ElectricityReportService(IPowerStationService powerStationService, IStationSystemService stationSystemService, ITemplateService templateService) {
            _powerStationService = powerStationService;
            _stationSystemService = stationSystemService;
            _templateService = templateService;
        }

        public List<ElectricityReportStationSummaryViewModel> GetElectricityReportStationSummaryData(DateTime startDate, DateTime dateTime) {
            List<ElectricityReportStationSummaryViewModel> ret = new List<ElectricityReportStationSummaryViewModel>();

            var allPs = _powerStationService.GetAllPowerStationInfos().FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0);
            foreach (var ps in allPs) {
                ElectricityReportStationSummaryViewModel electricityReportStationSummary = new ElectricityReportStationSummaryViewModel() {
                    PowerStationName = ps.Name
                };
                foreach (EnergyStorageCabinetInfo energyStorageCabinetInfo in ps.EnergyStorageCabinetRootDataList) {
                    List<Structure> rootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(energyStorageCabinetInfo.rootDataFromMqtt.structure, (int)DeviceCode.GatewayTableModel, 1);
                    List<GatewayTableModelInfo> pcsInfos = _stationSystemService.GetGatewayTableModelInfo(rootInfos.Select(S => S.name).ToList(), startDate, dateTime)
                        .GroupBy(m => new { m.DevName, Date = m.UploadTime.Date }).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();

                    electricityReportStationSummary.PeakForwardActiveEnergy += pcsInfos.Sum(s => s.PeakForwardActiveEnergy);
                    electricityReportStationSummary.PeakReverseActiveEnergy += pcsInfos.Sum(s => s.PeakReverseActiveEnergy);
                    electricityReportStationSummary.FlatForwardActiveEnergy += pcsInfos.Sum(s => s.FlatForwardActiveEnergy);
                    electricityReportStationSummary.FlatReverseActiveEnergy += pcsInfos.Sum(s => s.FlatReverseActiveEnergy);
                    electricityReportStationSummary.NormalForwardActiveEnergy += pcsInfos.Sum(s => s.NormalForwardActiveEnergy);
                    electricityReportStationSummary.NormalReverseActiveEnergy += pcsInfos.Sum(s => s.NormalReverseActiveEnergy);
                    electricityReportStationSummary.ValleyForwardActiveEnergy += pcsInfos.Sum(s => s.ValleyForwardActiveEnergy);
                    electricityReportStationSummary.ValleyReverseActiveEnergy += pcsInfos.Sum(s => s.ValleyReverseActiveEnergy);
                }
                ret.Add(electricityReportStationSummary);
            }

            return ret;
        }

        public ElectricityReportViewModel GetElectricityReportViewModel() {
            ElectricityReportViewModel ret = new ElectricityReportViewModel() {
                powerStationInfos = _powerStationService.GetAllPowerStationInfos().FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0)
            };


            return ret;
        }

        public Dictionary<DateTime, ElectricityReportByDay> GetSingleStationReportByDayData(int powerStationId, DateTime startDate, DateTime dateTime) {
            Dictionary<DateTime, ElectricityReportByDay> date2ElectricityReportData = new Dictionary<DateTime, ElectricityReportByDay>();
            var ps = _powerStationService.GetAllPowerStationInfos().Find(s => s.Id == powerStationId);
            if (ps == null) {
                return date2ElectricityReportData;
            }

            foreach (EnergyStorageCabinetInfo energyStorageCabinetInfo in ps.EnergyStorageCabinetRootDataList) {
                List<Structure> rootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(energyStorageCabinetInfo.rootDataFromMqtt.structure, (int)DeviceCode.GatewayTableModel, 1);
                List<GatewayTableModelInfo> infos = _stationSystemService.GetGatewayTableModelInfo(rootInfos.Select(S => S.name).ToList(), startDate, dateTime)
                        .GroupBy(m => new { m.DevName, Date = m.UploadTime.Date }).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();

                foreach (GatewayTableModelInfo info in infos) {
                    if (!date2ElectricityReportData.ContainsKey(info.UploadTime.Date)) {
                        date2ElectricityReportData[info.UploadTime.Date] = new ElectricityReportByDay() {
                            PeakForwardActiveEnergy = info.PeakForwardActiveEnergy,
                            PeakReverseActiveEnergy = info.PeakReverseActiveEnergy,
                            FlatForwardActiveEnergy = info.FlatForwardActiveEnergy,
                            FlatReverseActiveEnergy = info.FlatReverseActiveEnergy,
                            NormalForwardActiveEnergy = info.NormalForwardActiveEnergy,
                            NormalReverseActiveEnergy = info.NormalReverseActiveEnergy,
                            ValleyForwardActiveEnergy = info.ValleyForwardActiveEnergy,
                            ValleyReverseActiveEnergy = info.ValleyReverseActiveEnergy
                        };
                    } else {
                        date2ElectricityReportData[info.UploadTime.Date].PeakForwardActiveEnergy += info.PeakForwardActiveEnergy;
                        date2ElectricityReportData[info.UploadTime.Date].PeakReverseActiveEnergy += info.PeakReverseActiveEnergy;
                        date2ElectricityReportData[info.UploadTime.Date].FlatForwardActiveEnergy += info.FlatForwardActiveEnergy;
                        date2ElectricityReportData[info.UploadTime.Date].FlatReverseActiveEnergy += info.FlatReverseActiveEnergy;
                        date2ElectricityReportData[info.UploadTime.Date].NormalForwardActiveEnergy += info.NormalForwardActiveEnergy;
                        date2ElectricityReportData[info.UploadTime.Date].NormalReverseActiveEnergy += info.NormalReverseActiveEnergy;
                        date2ElectricityReportData[info.UploadTime.Date].ValleyForwardActiveEnergy += info.ValleyForwardActiveEnergy;
                        date2ElectricityReportData[info.UploadTime.Date].ValleyReverseActiveEnergy += info.ValleyReverseActiveEnergy;
                    }

                }
            }

            return date2ElectricityReportData.OrderBy(s => s.Key).ToDictionary(entry => entry.Key, entry => entry.Value);
        }
    }
}
