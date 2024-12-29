namespace IAMS.Models.StationSystem {
    public class EnergyStorageMeterInfo :DeviceDataBaseInfo{
        public double CriticalPeakChargingVolume { get; set; }
        public double PeakChargingVolume { get; set; }
        public double FlatChargingVolume { get; set; }
        public double ValleyChargingVolume { get; set; }
    }
}
