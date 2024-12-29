namespace IAMS.Models.StationSystem {
    public class EnergyStorageStackControlInfo: DeviceDataBaseInfo {
        public double DailyChargeCapacity { get; set; } = 0;
        public double DailyDischargeCapacity { get; set; } = 0;
        public double TotalChargeCapacity { get; set; } = 0;
        public double TotalDischargeCapacity { get; set; } = 0;
        public double SOC { get; set; } = 0;
        public double MaxChargePower { get; set; } = 0;
        public double MaxDischargePower { get; set; } = 0;
        public double MaxLoadPower { get; set; } = 0;

    }

}
