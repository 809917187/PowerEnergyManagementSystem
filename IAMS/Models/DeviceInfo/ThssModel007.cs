using IAMS.AttributeTag;
using System.ComponentModel.DataAnnotations;

namespace IAMS.Models.DeviceInfo {
    public class ThssModel007 {
        [Display(Name = "是否在线"), PointIndex(0)]
        public int OnlineStatus { get; set; }

        [Display(Name = "温度"), PointIndex(1)]
        public int Temperature { get; set; }

        [Display(Name = "湿度"), PointIndex(2)]
        public int Humidity { get; set; }

        [Display(AutoGenerateField = false), PointRange(4, 19)]
        public int[] Reserved { get; set; }

    }
}
