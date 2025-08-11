using IAMS.Models.DeviceInfo;
using IAMS.MQTT;
using IAMS.MQTT.Model;
using IAMS.ViewModels.DeviceMonitor;

namespace IAMS.Service {
    public class DeviceMonitorService : IDeviceMonitorService {
        private string _connectionString;
        private IPowerStationService _powerStationService;
        private IStationSystemService _stationSystemServicee;
        private IClickHouseService _clickHouseService;
        public DeviceMonitorService(IConfiguration configuration, IPowerStationService powerStationService,
                    IStationSystemService stationSystemService, IClickHouseService clickHouseService) {
            _connectionString = configuration.GetConnectionString("gq");
            _powerStationService = powerStationService;
            _stationSystemServicee = stationSystemService;
            _clickHouseService = clickHouseService;
        }
        public DeviceMonitorViewModel GetDeviceMonitorViewModel(string energyStorageCabinetName, DateTime dateTime) {
            DeviceMonitorViewModel model = new DeviceMonitorViewModel();
            //model.PowerStationInfos = _powerStationService.GetAllPowerStationInfoByCabinetName(energyStorageCabinetName);

            List<DeviceBaseInfo> deviceInfos = _powerStationService.GetDeviceBaseInfosByPowerStationId(model.PowerStationInfos.Select(s => s.Id).ToList());

            model.PcsInfos = _clickHouseService.GetPcsModel005s(deviceInfos.Where(s => s.DeviceType == (int)DeviceCode.PCS).Select(s => s.Sn).ToList(), dateTime);
            model.EnergyStorageStackControlInfos = _clickHouseService.GetBsuModel003s(deviceInfos.Where(s => s.DeviceType == (int)DeviceCode.BSU).Select(s => s.Sn).ToList(), dateTime);
            model.GatewayTableModelInfos = _clickHouseService.GetPccModel001s(deviceInfos.Where(s => s.DeviceType == (int)DeviceCode.PCC).Select(s => s.Sn).ToList(), dateTime);

            return model;
        }
    }
}
