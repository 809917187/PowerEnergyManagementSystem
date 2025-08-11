using IAMS.AttributeTag;
using System.ComponentModel.DataAnnotations;

namespace IAMS.Models.DeviceInfo {
    public class AirlqdModel006 : DeviceBaseInfo {
        [Display(Name = "是否在线"), PointIndex(0)]
        public int OnlineStatus { get; set; }

        [Display(Name = "告警1"), PointIndex(1)]
        public int Alarm1 { get; set; }

        [Display(Name = "告警2"), PointIndex(2)]
        public int Alarm2 { get; set; }

        [Display(Name = "告警3"), PointIndex(3)]
        public int Alarm3 { get; set; }

        [Display(Name = "告警4"), PointIndex(4)]
        public int Alarm4 { get; set; }

        [Display(Name = "告警5"), PointIndex(5)]
        public int Alarm5 { get; set; }

        [Display(Name = "告警6"), PointIndex(6)]
        public int Alarm6 { get; set; }

        [Display(Name = "告警7"), PointIndex(7)]
        public int Alarm7 { get; set; }

        [Display(Name = "告警8"), PointIndex(8)]
        public int Alarm8 { get; set; }

        [Display(Name = "告警9"), PointIndex(9)]
        public int Alarm9 { get; set; }

        [Display(Name = "告警10"), PointIndex(10)]
        public int Alarm10 { get; set; }

        [Display(Name = "告警11"), PointIndex(11)]
        public int Alarm11 { get; set; }

        [Display(Name = "告警12"), PointIndex(12)]
        public int Alarm12 { get; set; }

        [Display(Name = "告警13"), PointIndex(13)]
        public int Alarm13 { get; set; }

        [Display(Name = "告警14"), PointIndex(14)]
        public int Alarm14 { get; set; }

        [Display(Name = "告警15"), PointIndex(15)]
        public int Alarm15 { get; set; }

        [Display(Name = "告警16"), PointIndex(16)]
        public int Alarm16 { get; set; }

        [Display(Name = "告警17"), PointIndex(17)]
        public int Alarm17 { get; set; }

        [Display(Name = "告警18"), PointIndex(18)]
        public int Alarm18 { get; set; }

        [Display(Name = "告警19"), PointIndex(19)]
        public int Alarm19 { get; set; }

        [Display(Name = "告警20"), PointIndex(20)]
        public int Alarm20 { get; set; }

        [Display(Name = "告警21"), PointIndex(21)]
        public int Alarm21 { get; set; }

        [Display(Name = "告警22"), PointIndex(22)]
        public int Alarm22 { get; set; }

        [Display(Name = "告警23"), PointIndex(23)]
        public int Alarm23 { get; set; }

        [Display(Name = "告警24"), PointIndex(24)]
        public int Alarm24 { get; set; }

        [Display(Name = "告警25"), PointIndex(25)]
        public int Alarm25 { get; set; }

        [Display(Name = "告警26"), PointIndex(26)]
        public int Alarm26 { get; set; }

        [Display(Name = "告警27"), PointIndex(27)]
        public int Alarm27 { get; set; }

        [Display(Name = "告警28"), PointIndex(28)]
        public int Alarm28 { get; set; }

        [Display(Name = "告警29"), PointIndex(29)]
        public int Alarm29 { get; set; }

        [Display(Name = "告警30"), PointIndex(30)]
        public int Alarm30 { get; set; }

        [Display(Name = "告警31"), PointIndex(31)]
        public int Alarm31 { get; set; }

        [Display(Name = "告警32"), PointIndex(32)]
        public int Alarm32 { get; set; }

        [Display(Name = "告警33"), PointIndex(33)]
        public int Alarm33 { get; set; }

        [Display(Name = "告警34"), PointIndex(34)]
        public int Alarm34 { get; set; }

        [Display(Name = "告警35"), PointIndex(35)]
        public int Alarm35 { get; set; }

        [Display(Name = "告警36"), PointIndex(36)]
        public int Alarm36 { get; set; }

        [Display(Name = "告警37"), PointIndex(37)]
        public int Alarm37 { get; set; }

        [Display(Name = "告警38"), PointIndex(38)]
        public int Alarm38 { get; set; }

        [Display(Name = "告警39"), PointIndex(39)]
        public int Alarm39 { get; set; }

        [Display(Name = "告警40"), PointIndex(40)]
        public int Alarm40 { get; set; }

        [Display(Name = "温度"), PointIndex(41)]
        public int Temperature { get; set; }

        [Display(Name = "湿度"), PointIndex(42)]
        public int Humidity { get; set; }

        [Display(AutoGenerateField = false), PointRange(43, 70)]
        public int[] TelemetryReserved { get; set; }

        [Display(Name = "制冷温度"), PointIndex(71)]
        public int CoolingTemperature { get; set; }

        [Display(Name = "制热温度"), PointIndex(72)]
        public int HeatingTemperature { get; set; }

        [Display(Name = "制冷回差"), PointIndex(73)]
        public int CoolingHysteresis { get; set; }

        [Display(Name = "制热回差"), PointIndex(74)]
        public int HeatingHysteresis { get; set; }

        [Display(Name = "运行模式"), PointIndex(75)]
        public int OperationMode { get; set; }

        [Display(Name = "设备开关、制冷制热"), PointIndex(76)]
        public int DeviceSwitchCoolingHeating { get; set; }

        [Display(Name = "电芯最高温度"), PointIndex(77)]
        public int MaxCellTemperature { get; set; }

        [Display(Name = "电芯最低温度"), PointIndex(78)]
        public int MinCellTemperature { get; set; }

        [Display(Name = "电芯平均温度"), PointIndex(79)]
        public int AvgCellTemperature { get; set; }

        [Display(Name = "液冷机组运行模式"), PointIndex(80)]
        public int LiquidCoolingUnitOperationMode { get; set; }

        [Display(Name = "液冷机组开关机"), PointIndex(81)]
        public int LiquidCoolingUnitPowerSwitch { get; set; }

        [Display(Name = "液冷运行温度"), PointIndex(82)]
        public int LiquidCoolingOperatingTemperature { get; set; }

        [Display(AutoGenerateField = false), PointRange(83, 99)]
        public int[] SettingReserved { get; set; }

    }
}
