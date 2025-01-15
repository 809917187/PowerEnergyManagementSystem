using IAMS.MQTT;
using IAMS.MQTT.Model;
using IAMS.ViewModels.DeviceMonitor;

namespace IAMS.Service {
    public class DeviceMonitorService : IDeviceMonitorService {
        private string _connectionString;
        private IPowerStationService _powerStationService;
        private IStationSystemService _stationSystemServicee;
        public DeviceMonitorService(IConfiguration configuration, IPowerStationService powerStationService,IStationSystemService stationSystemService) {
            _connectionString = configuration.GetConnectionString("gq");
            _powerStationService = powerStationService;
            _stationSystemServicee = stationSystemService;
        }
        public DeviceMonitorViewModel GetDeviceMonitorViewModel(string energyStorageCabinetName, DateTime dateTime) {
            DeviceMonitorViewModel model = new DeviceMonitorViewModel();
            model.PowerStationInfos = _powerStationService.GetAllPowerStationInfoByCabinetName(energyStorageCabinetName);

            RootDataFromMqtt viewDataSourceCabinet = _powerStationService.GetDataSourceCabinet(model.PowerStationInfos);
            //PCS devType=5
            List<Structure> pcsRootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(viewDataSourceCabinet.structure, (int)DeviceCode.PCS, 1);
            model.PcsInfos = _stationSystemServicee.GetPCSInfo(pcsRootInfos.Select(S => S.name).ToList(), dateTime).GroupBy(m => m.DevName).Select(g => g.OrderByDescending(m => m.Id).First()).ToList();
            //储能堆控
            List<Structure> bsuRootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(viewDataSourceCabinet.structure, (int)DeviceCode.EnergyStorageStackControl, 1);
            model.EnergyStorageStackControlInfos = _stationSystemServicee.GetEnergyStorageStackControllerInfo(bsuRootInfos.Select(S => S.name).ToList(), dateTime).GroupBy(m => m.DevName).Select(g => g.OrderByDescending(m => m.Id).First()).ToList();
            //关口表
            List<Structure> gatewayTableRootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(viewDataSourceCabinet.structure, (int)DeviceCode.GatewayTableModel, 1);
            model.GatewayTableModelInfos = _stationSystemServicee.GetGatewayTableModelInfo(gatewayTableRootInfos.Select(S => S.name).ToList(), dateTime).GroupBy(m => m.DevName).Select(g => g.OrderByDescending(m => m.Id).First()).ToList();

            return model;
        }
    }
}
