using IAMS.MQTT.Model;

namespace IAMS.Models.PowerStation {
    public class PowerStationRootInfo : RootDataFromMqtt {
        public int PowerStationId { get; set; }
        public RootDataFromMqtt rootDataFromMqtt { get; set; }
    }
}
