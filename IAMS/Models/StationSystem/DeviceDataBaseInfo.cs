namespace IAMS.Models.StationSystem {
    public class DeviceDataBaseInfo {
        public int Id { get; set; }
        public DateTime UploadTime { get; set; }
        public string DevType { get; set; } = string.Empty;
        public string DevName { get; set; } = string.Empty;
        public string DevId { get; set; } = string.Empty;
        public string Sn { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int PowerStationId { get; set; }
    }
}
