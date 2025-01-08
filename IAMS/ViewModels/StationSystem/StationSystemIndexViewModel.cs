using IAMS.Models.PowerStation;
using IAMS.Models.StationSystem;
using IAMS.MQTT.Model;

namespace IAMS.ViewModels.StationSystem {
    public class StationSystemIndexViewModel {
        public List<PowerStationInfo> PowerStationInfos { get; set; }
        public List<PowerStationRootInfo> EnergyStorageCabinets { get; set; }
        public PowerStationInfo selectedPowerStation { get; set; }
        public PowerStationRootInfo selectedEnergyStorageCabinet { get; set; }
        public List<PCSInfo> pcsInfos { get; set; }


    }

}
