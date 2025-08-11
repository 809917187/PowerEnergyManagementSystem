using IAMS.AttributeTag;
using System.ComponentModel.DataAnnotations;

namespace IAMS.Models.DeviceInfo {
    public class FireModel010 : DeviceBaseInfo {
        [Display(Name = "是否启用"), PointIndex(0)]
        public int IsEnabled { get; set; }

        [Display(Name = "是否在线"), PointIndex(1)]
        public int OnlineStatus { get; set; }

        [Display(AutoGenerateField = false), PointRange(3, 199)]
        public int[] Reserved { get; set; }

    }
}
