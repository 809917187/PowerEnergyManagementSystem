using IAMS.Models.PowerStation;

namespace IAMS.ViewModels.MultiStationOverview {
    public class MultiStationOverviewViewModel {
        public List<PowerStationInfo> OnlinePowerStation { get; set; } = new List<PowerStationInfo>();
        public List<PowerStationInfo> OfflinePowerStation { get; set; } = new List<PowerStationInfo>();

        public int PowerStationTotalNumber { get; set; }
        public int PowerStationOnlineNumber { get; set; }
        public int PowerStationOfflineNumber { get; set; }
        public int PowerStationToBeMaintainanceNumber { get; set; }

        public double InstalledPower { get; set; }
        public double InstalledCapacity { get; set; }

        public double ChargeAmountToday { get; set; } = 0;
        public double ChargeAmountRaise { get; set; } = 0;
        public double ChargeAmountThisMonth { get; set; } = 0;
        public double TotalChargeAmount { get; set; } = 0;
        public double DischargeAmountToday { get; set; } = 0;
        public double DischargeAmountRaise { get; set; } = 0;
        public double DischargeAmountThisMonth { get; set; } = 0;
        public double TotalDischargeAmount { get; set; } = 0;
        public decimal EarningToday { get; set; } = 0;
        public decimal EarningRaise { get; set; } = 0;
        public decimal EarningThisMonth { get; set; } = 0;
        public decimal TotalEarning { get; set; } = 0;
    }
}
