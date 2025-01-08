using IAMS.MQTT.Model;

namespace IAMS.Models.PowerStation {
    public class PowerStationInfo {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public float InstalledPower { get; set; }
        public float InstalledCapacity { get; set; }
        public DateTime StartTime { get; set; }
        public string Country { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string LocationDetails { get; set; } = string.Empty;
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public float TransformerCapacity { get; set; }
        public string TransformerInfo { get; set; } = string.Empty;
        public string NetworkInfo { get; set; } = string.Empty;
        public string Installer { get; set; } = string.Empty;
        public string InstallerPhone { get; set; } = string.Empty;
        public List<IFormFile> StationImages { get; set; } = new List<IFormFile>();
        public List<string> StationImagesFilePath { get; set; } = new List<string>();
        public List<IFormFile> StationInstallImages { get; set; } = new List<IFormFile>();
        public List<string> StationInstallImagesFilePath { get; set; } = new List<string>();

        public List<RootDataFromMqtt> EnergyStorageCabinetList { get; set; } = new List<RootDataFromMqtt>();
    }
}
