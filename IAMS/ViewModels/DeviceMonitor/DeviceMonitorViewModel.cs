using IAMS.Models.PowerStation;
using IAMS.Models.StationSystem;

namespace IAMS.ViewModels.DeviceMonitor {
    public class DeviceMonitorViewModel {
        public List<PowerStationInfo> PowerStationInfos { get; set; } = new List<PowerStationInfo>();
        public List<PCSInfo> PcsInfos { get; set; }
        public List<GatewayTableModelInfo> GatewayTableModelInfos { get; set; }
        public List<EnergyStorageStackControlInfo> EnergyStorageStackControlInfos { get; set; }
    }

}
