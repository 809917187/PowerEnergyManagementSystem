using IAMS.AttributeTag;
using System.ComponentModel.DataAnnotations;

namespace IAMS.Models.DeviceInfo {
    public class GasModel009: DeviceBaseInfo {
        [Display(Name = "是否在线"), PointIndex(0)]
        public int OnlineStatus { get; set; }

        [Display(Name = "水浸告警"), PointIndex(1)]
        public int WaterLeakAlarm { get; set; }

        [Display(AutoGenerateField = false), PointRange(2, 19)]
        public int[] Reserved { get; set; }

    }
}
