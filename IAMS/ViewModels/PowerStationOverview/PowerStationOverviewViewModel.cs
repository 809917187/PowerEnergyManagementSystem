using IAMS.Models.PowerStation;

namespace IAMS.ViewModels.PowerStationOverview {
    public class PowerStationOverviewViewModel {
        public List<PowerStationInfo> PowerStationInfos { get; set; } = new List<PowerStationInfo>();
        public PowerStationInfo SelectedPowerStation { get; set; } = new PowerStationInfo();
        public double ChargeAmountToday { get; set; }
        //public double ChargeAmountYesterday { get; set; }
        public double ChargeAmountRaise { get; set; }
        public double ChargeAmountThisMonth { get; set; }
        public double TotalChargeAmount { get; set; }

        public double DischargeAmountToday { get; set; }
        //public double DischargeAmountYesterday { get; set; }
        public double DischargeAmountRaise { get; set; }
        public double DischargeAmountThisMonth { get; set; }
        public double TotalDischargeAmount { get; set; }

        public decimal EarningToday { get; set; }
        //public double EarningYesterday { get; set; }
        public decimal EarningRaise { get; set; }
        public decimal EarningThisMonth { get; set; }
        public decimal TotalEarning { get; set; }

        public int AlarmCount { get; set; }
        public int MaintenanceDealingCount { get; set; }
        public int NoNeedDealingCount { get; set; }
        public List<CabinetStationSystemInfo> CabinetStationSystemInfos { get; set; } = new List<CabinetStationSystemInfo>();
    }

    

}
