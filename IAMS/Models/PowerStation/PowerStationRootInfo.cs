using IAMS.MQTT.Model;

namespace IAMS.Models.PowerStation {
    public class PowerStationRootInfo  {
        public int PowerStationId { get; set; }
        public RootDataFromMqtt rootDataFromMqtt { get; set; }
    }
}
