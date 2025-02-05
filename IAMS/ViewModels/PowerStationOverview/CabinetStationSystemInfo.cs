namespace IAMS.ViewModels.PowerStationOverview {
    public class CabinetStationSystemInfo {
        public string CabinetName { get; set; } = string.Empty;
        public string SystemName { get; set; } = string.Empty;
        public string OnlineStatus { get; set; } = "在线";
        public double DailyDischargeAmount { get; set; } = 0;
    }
}
