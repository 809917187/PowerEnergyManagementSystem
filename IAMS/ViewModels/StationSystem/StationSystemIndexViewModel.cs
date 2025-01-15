using IAMS.Models.PowerStation;
using IAMS.Models.StationSystem;
using IAMS.MQTT.Model;

namespace IAMS.ViewModels.StationSystem {
    public class StationSystemIndexViewModel {
        public List<PowerStationInfo> PowerStationInfos { get; set; } = new List<PowerStationInfo>();
        public string DailyChargeAmount { get; set; } = String.Empty;
        public string DailyDischargeAmount { get; set; } = String.Empty;
        public string ACCumulativeChargeAmount { get; set; } = String.Empty;
        public string ACCumulativeDischargeAmount { get; set; } = String.Empty;
        public string SOC { get; set; } = String.Empty;
        public string PowerGrid { get; set; } = String.Empty;
        public string Load { get; set; } = String.Empty;
        public string EnergyStorage { get; set; } = String.Empty;
        public List<(string devName, string status)> deviceStatus { get; set; } = new List<(string devName, string status)>();


    }

}
