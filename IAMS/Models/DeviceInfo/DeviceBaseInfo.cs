using IAMS.AttributeTag;
using System.ComponentModel.DataAnnotations;

namespace IAMS.Models.DeviceInfo {
    public class DeviceBaseInfo {
        [Display(Name = "SN"), NotPointData]
        public string Sn { get; set; } = string.Empty;

        [Display(Name = "时间"), NotPointData]
        public DateTime UploadTime { get; set; }

        [Display(AutoGenerateField = false), NotPointData]
        public int DeviceType { get; set; }
        [Display(Name = "设备名称"), NotPointData]
        public string DeviceName { get; set; }
        [Display(AutoGenerateField = false), NotPointData]
        public string DeviceId { get; set; } = string.Empty;
        [Display(AutoGenerateField = false), NotPointData]
        public string emsSn { get; set; }
        [Display(AutoGenerateField = false), NotPointData]
        public int? PowerStationId { get; set; }
    }
}
