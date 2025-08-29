using IAMS.Common;
using IAMS.Models.DeviceInfo;
using IAMS.Models.PowerStation;
using IAMS.MQTT;
using IAMS.MQTT.Model;
using IAMS.ViewModels.ElectricityReport;

namespace IAMS.Service {
    public class ElectricityReportService : IElectricityReportService {
        private IPowerStationService _powerStationService;
        private IStationSystemService _stationSystemService;
        private ITemplateService _templateService;
        private IClickHouseService _clickHouseService;
        public ElectricityReportService(IPowerStationService powerStationService, IStationSystemService stationSystemService,
            ITemplateService templateService, IClickHouseService clickHouseService) {
            _powerStationService = powerStationService;
            _stationSystemService = stationSystemService;
            _templateService = templateService;
            _clickHouseService = clickHouseService;
        }

        public List<ElectricityReportStationSummaryViewModel> GetElectricityReportStationSummaryData(DateTime startDate, DateTime dateTime) {
            List<ElectricityReportStationSummaryViewModel> ret = new List<ElectricityReportStationSummaryViewModel>();

            var allPs = _powerStationService.GetAllPowerStationInfos(0,null).FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0);

            var deviceBaseInfos = _powerStationService.GetDeviceBaseInfosByPowerStationId(allPs.Select(s => s.Id).ToList());

            foreach (var ps in allPs) {
                ElectricityReportStationSummaryViewModel electricityReportStationSummary = new ElectricityReportStationSummaryViewModel() {
                    PowerStationName = ps.Name
                };

                List<PccModel001> pccInfos = _clickHouseService.GetPccModel001s(deviceBaseInfos.Where(s => s.DeviceType == (int)DeviceCode.PCC && s.PowerStationId == ps.Id).Select(s => s.Sn).ToList(), DateTime.Today);

                electricityReportStationSummary.PeakForwardActiveEnergy = pccInfos.Sum(s => s.PeakForwardActiveEnergy);
                electricityReportStationSummary.PeakReverseActiveEnergy = pccInfos.Sum(s => s.PeakReverseActiveEnergy);
                electricityReportStationSummary.HighForwardActiveEnergy = pccInfos.Sum(s => s.HighForwardActiveEnergy);
                electricityReportStationSummary.HighReverseActiveEnergy = pccInfos.Sum(s => s.HighReverseActiveEnergy);
                electricityReportStationSummary.FlatForwardActiveEnergy = pccInfos.Sum(s => s.FlatForwardActiveEnergy);
                electricityReportStationSummary.FlatReverseActiveEnergy = pccInfos.Sum(s => s.FlatReverseActiveEnergy);
                electricityReportStationSummary.ValleyForwardActiveEnergy = pccInfos.Sum(s => s.ValleyForwardActiveEnergy);
                electricityReportStationSummary.ValleyReverseActiveEnergy = pccInfos.Sum(s => s.ValleyReverseActiveEnergy);
                ret.Add(electricityReportStationSummary);
            }

            return ret;
        }

        public ElectricityReportViewModel GetElectricityReportViewModel() {
            ElectricityReportViewModel ret = new ElectricityReportViewModel() {
                powerStationInfos = _powerStationService.GetAllPowerStationInfos(0,null).FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0)
            };


            return ret;
        }

        public Dictionary<DateTime, ElectricityReportByDay> GetSingleStationReportByDayData(int powerStationId, DateTime startDate, DateTime dateTime) {
            Dictionary<DateTime, ElectricityReportByDay> date2ElectricityReportData = new Dictionary<DateTime, ElectricityReportByDay>();
            var ps = _powerStationService.GetAllPowerStationInfos(powerStationId,null).Find(s => s.Id == powerStationId);
            if (ps == null) {
                return date2ElectricityReportData;
            }
            var deviceBaseInfos = _powerStationService.GetDeviceBaseInfosByPowerStationId(new List<int>() { ps.Id });

            foreach (EnergyStorageCabinetInfo energyStorageCabinetInfo in ps.EnergyStorageCabinetRootDataList) {
                List<PccModel001> infos = _clickHouseService.GetPccModel001s(deviceBaseInfos.Where(s => s.DeviceType == (int)DeviceCode.PCC).Select(s => s.Sn).ToList(), startDate, dateTime)
                        .GroupBy(m => new { m.Sn, Date = m.UploadTime.Date }).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();

                foreach (PccModel001 info in infos) {
                    if (!date2ElectricityReportData.ContainsKey(info.UploadTime.Date)) {
                        date2ElectricityReportData[info.UploadTime.Date] = new ElectricityReportByDay() {
                            PeakForwardActiveEnergy = info.PeakForwardActiveEnergy,
                            PeakReverseActiveEnergy = info.PeakReverseActiveEnergy,
                            FlatForwardActiveEnergy = info.FlatForwardActiveEnergy,
                            FlatReverseActiveEnergy = info.FlatReverseActiveEnergy,
                            HighForwardActiveEnergy = info.HighForwardActiveEnergy,
                            HighReverseActiveEnergy = info.HighReverseActiveEnergy,
                            ValleyForwardActiveEnergy = info.ValleyForwardActiveEnergy,
                            ValleyReverseActiveEnergy = info.ValleyReverseActiveEnergy
                        };
                    } else {
                        date2ElectricityReportData[info.UploadTime.Date].PeakForwardActiveEnergy += info.PeakForwardActiveEnergy;
                        date2ElectricityReportData[info.UploadTime.Date].PeakReverseActiveEnergy += info.PeakReverseActiveEnergy;
                        date2ElectricityReportData[info.UploadTime.Date].FlatForwardActiveEnergy += info.FlatForwardActiveEnergy;
                        date2ElectricityReportData[info.UploadTime.Date].FlatReverseActiveEnergy += info.FlatReverseActiveEnergy;
                        date2ElectricityReportData[info.UploadTime.Date].HighForwardActiveEnergy += info.HighForwardActiveEnergy;
                        date2ElectricityReportData[info.UploadTime.Date].HighReverseActiveEnergy += info.HighReverseActiveEnergy;
                        date2ElectricityReportData[info.UploadTime.Date].ValleyForwardActiveEnergy += info.ValleyForwardActiveEnergy;
                        date2ElectricityReportData[info.UploadTime.Date].ValleyReverseActiveEnergy += info.ValleyReverseActiveEnergy;
                    }

                }
            }

            return date2ElectricityReportData.OrderBy(s => s.Key).ToDictionary(entry => entry.Key, entry => entry.Value);
        }
    }
}
