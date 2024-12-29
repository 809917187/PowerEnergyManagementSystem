using IAMS.Models.PowerStation;

namespace IAMS.ViewModels.Home {
    public class MultiPowerStationOverviewViewModel {
        public int OnlinePowerSattionNumber { set; get; }
        public int OfflinePowerSattionNumber { set; get; }
        public int TobeMaintaincePowerSattionNumber { set; get; }
        public float InstalledPower { get; set; }
        public float InstalledCapacity { get; set; }

    }
}
