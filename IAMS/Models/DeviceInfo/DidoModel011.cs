using IAMS.AttributeTag;
using System.ComponentModel.DataAnnotations;

namespace IAMS.Models.DeviceInfo {
    public class DidoModel011 : DeviceBaseInfo {
        [Display(Name = "是否在线"), PointIndex(0)]
        public int OnlineStatus { get; set; }

        [Display(Name = "1#DI信号"), PointIndex(1)]
        public int DI1 { get; set; }
        [Display(Name = "2#DI信号"), PointIndex(2)]
        public int DI2 { get; set; }
        [Display(Name = "3#DI信号"), PointIndex(3)]
        public int DI3 { get; set; }
        [Display(Name = "4#DI信号"), PointIndex(4)]
        public int DI4 { get; set; }
        [Display(Name = "5#DI信号"), PointIndex(5)]
        public int DI5 { get; set; }
        [Display(Name = "6#DI信号"), PointIndex(6)]
        public int DI6 { get; set; }
        [Display(Name = "7#DI信号"), PointIndex(7)]
        public int DI7 { get; set; }
        [Display(Name = "8#DI信号"), PointIndex(8)]
        public int DI8 { get; set; }
        [Display(Name = "9#DI信号"), PointIndex(9)]
        public int DI9 { get; set; }
        [Display(Name = "10#DI信号"), PointIndex(10)]
        public int DI10 { get; set; }
        [Display(Name = "11#DI信号"), PointIndex(11)]
        public int DI11 { get; set; }
        [Display(Name = "12#DI信号"), PointIndex(12)]
        public int DI12 { get; set; }
        [Display(Name = "13#DI信号"), PointIndex(13)]
        public int DI13 { get; set; }
        [Display(Name = "14#DI信号"), PointIndex(14)]
        public int DI14 { get; set; }
        [Display(Name = "15#DI信号"), PointIndex(15)]
        public int DI15 { get; set; }
        [Display(Name = "16#DI信号"), PointIndex(16)]
        public int DI16 { get; set; }
        [Display(Name = "17#DI信号"), PointIndex(17)]
        public int DI17 { get; set; }
        [Display(Name = "18#DI信号"), PointIndex(18)]
        public int DI18 { get; set; }
        [Display(Name = "19#DI信号"), PointIndex(19)]
        public int DI19 { get; set; }
        [Display(Name = "20#DI信号"), PointIndex(20)]
        public int DI20 { get; set; }

        [Display(Name = "1#DO输出"), PointIndex(21)]
        public int DO1 { get; set; }
        [Display(Name = "2#DO输出"), PointIndex(22)]
        public int DO2 { get; set; }
        [Display(Name = "3#DO输出"), PointIndex(23)]
        public int DO3 { get; set; }
        [Display(Name = "4#DO输出"), PointIndex(24)]
        public int DO4 { get; set; }
        [Display(Name = "5#DO输出"), PointIndex(25)]
        public int DO5 { get; set; }
        [Display(Name = "6#DO输出"), PointIndex(26)]
        public int DO6 { get; set; }
        [Display(Name = "7#DO输出"), PointIndex(27)]
        public int DO7 { get; set; }
        [Display(Name = "8#DO输出"), PointIndex(28)]
        public int DO8 { get; set; }
        [Display(Name = "9#DO输出"), PointIndex(29)]
        public int DO9 { get; set; }
        [Display(Name = "10#DO输出"), PointIndex(30)]
        public int DO10 { get; set; }
        [Display(Name = "11#DO输出"), PointIndex(31)]
        public int DO11 { get; set; }
        [Display(Name = "12#DO输出"), PointIndex(32)]
        public int DO12 { get; set; }
        [Display(Name = "13#DO输出"), PointIndex(33)]
        public int DO13 { get; set; }
        [Display(Name = "14#DO输出"), PointIndex(34)]
        public int DO14 { get; set; }
        [Display(Name = "15#DO输出"), PointIndex(35)]
        public int DO15 { get; set; }
        [Display(Name = "16#DO输出"), PointIndex(36)]
        public int DO16 { get; set; }
        [Display(Name = "17#DO输出"), PointIndex(37)]
        public int DO17 { get; set; }
        [Display(Name = "18#DO输出"), PointIndex(38)]
        public int DO18 { get; set; }
        [Display(Name = "19#DO输出"), PointIndex(39)]
        public int DO19 { get; set; }
        [Display(Name = "20#DO输出"), PointIndex(40)]
        public int DO20 { get; set; }

        [Display(AutoGenerateField = false), PointRange(41, 99)]
        public int[] Reserved { get; set; }

    }
}
