using IAMS.Models.DeviceInfo;
using IAMS.Models.PowerStation;

namespace IAMS.ViewModels.DeviceMonitor {
    public class DeviceMonitorViewModel {
        public List<PowerStationInfo> PowerStationInfos { get; set; } = new List<PowerStationInfo>();
        public List<PcsModel005> PcsInfos { get; set; }
        public List<PccModel001> GatewayTableModelInfos { get; set; }
        public List<BsuModel003> EnergyStorageStackControlInfos { get; set; }
    }

}
