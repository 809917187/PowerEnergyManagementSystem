using IAMS.MQTT.Model;

namespace IAMS.Models.PowerStation {
    public class EnergyStorageCabinetInfo {
        public bool IsSelected { get; set; }
        public RootDataFromMqtt rootDataFromMqtt { get; set; }
        public string CabinetName { get; set; }
        public int CabinetId { get; set; }
        public int? PowerStationId { get; set; }
    }
}
