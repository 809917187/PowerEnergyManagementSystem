using IAMS.MQTT.Model;

namespace IAMS.Models.PowerStation {
    public class EnergyStorageCabinetInfo {
        public bool IsSelected { get; set; }
        public string CabinetSn { get; set; }
        public int CabinetId { get; set; }
        public int? PowerStationId { get; set; }
    }
}
