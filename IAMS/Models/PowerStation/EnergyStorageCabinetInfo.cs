using IAMS.MQTT.Model;

namespace IAMS.Models.PowerStation {
    public class EnergyStorageCabinetInfo {
        public bool IsSelected { get; set; }
        public RootDataFromMqtt rootDataFromMqtt { get; set; }
    }
}
