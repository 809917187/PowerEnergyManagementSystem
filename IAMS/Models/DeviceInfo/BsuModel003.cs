using IAMS.AttributeTag;
using System.ComponentModel.DataAnnotations;

namespace IAMS.Models.DeviceInfo {
    public class BsuModel003: DeviceBaseInfo {
        [Display(Name = "在线状态"), PointIndex(0)]
        public bool OnlineStatus { get; set; }

        [Display(Name = "总告警"), PointIndex(1)]
        public int TotalAlarm { get; set; }

        [Display(Name = "总故障"), PointIndex(2)]
        public int TotalFault { get; set; }

        [Display(Name = "总压"), PointIndex(3)]
        public int TotalVoltage { get; set; }

        [Display(Name = "总电流"), PointIndex(4)]
        public int TotalCurrent { get; set; }

        [Display(Name = "SOC"), PointIndex(5)]
        public int SOC { get; set; }

        [Display(Name = "SOH"), PointIndex(6)]
        public int SOH { get; set; }

        [Display(Name = "SOE"), PointIndex(7)]
        public int SOE { get; set; }

        [Display(Name = "额定总压"), PointIndex(8)]
        public int RatedTotalVoltage { get; set; }

        [Display(Name = "额定容量"), PointIndex(9)]
        public int RatedCapacity { get; set; }

        [Display(Name = "剩余容量"), PointIndex(10)]
        public int RemainingCapacity { get; set; }

        [Display(Name = "额定电量"), PointIndex(11)]
        public int RatedEnergy { get; set; }

        [Display(Name = "剩余电量"), PointIndex(12)]
        public int RemainingEnergy { get; set; }

        [Display(Name = "从机总数(BMU)"), PointIndex(13)]
        public int TotalSlaveCountBMU { get; set; }

        [Display(Name = "在线从机总数(BMU)"), PointIndex(14)]
        public int OnlineSlaveCountBMU { get; set; }

        [Display(Name = "电池总数"), PointIndex(15)]
        public int TotalBatteryCount { get; set; }

        [Display(Name = "在线电池总数"), PointIndex(16)]
        public int OnlineBatteryCount { get; set; }

        [Display(Name = "温感总数"), PointIndex(17)]
        public int TotalTemperatureSensorCount { get; set; }

        [Display(Name = "在线温感总数"), PointIndex(18)]
        public int OnlineTemperatureSensorCount { get; set; }

        [Display(Name = "最大允许放电电流"), PointIndex(19)]
        public int MaxAllowedDischargeCurrent { get; set; }

        [Display(Name = "最大允许放电功率"), PointIndex(20)]
        public int MaxAllowedDischargePower { get; set; }

        [Display(Name = "最大允许充电电流"), PointIndex(21)]
        public int MaxAllowedChargeCurrent { get; set; }

        [Display(Name = "最大允许充电功率"), PointIndex(22)]
        public int MaxAllowedChargePower { get; set; }

        [Display(Name = "正极绝缘阻值"), PointIndex(23)]
        public int PositiveInsulationResistance { get; set; }

        [Display(Name = "负极绝缘阻值"), PointIndex(24)]
        public int NegativeInsulationResistance { get; set; }

        [Display(Name = "单体平均电压"), PointIndex(25)]
        public int SingleCellAverageVoltage { get; set; }

        [Display(Name = "单体最大压差"), PointIndex(26)]
        public int SingleCellMaxVoltageDifference { get; set; }

        [Display(Name = "最高单体电压"), PointIndex(27)]
        public int HighestSingleCellVoltage { get; set; }

        [Display(Name = "最高单体电压从机号"), PointIndex(28)]
        public int HighestSingleCellVoltageSlaveId { get; set; }

        [Display(Name = "最高单体电压编号"), PointIndex(29)]
        public int HighestSingleCellVoltageId { get; set; }

        [Display(Name = "最低单体电压"), PointIndex(30)]
        public int LowestSingleCellVoltage { get; set; }

        [Display(Name = "最低单体电压从机号"), PointIndex(31)]
        public int LowestSingleCellVoltageSlaveId { get; set; }

        [Display(Name = "最低单体电压编号"), PointIndex(32)]
        public int LowestSingleCellVoltageId { get; set; }

        [Display(Name = "单体平均温度"), PointIndex(33)]
        public int SingleCellAverageTemperature { get; set; }

        [Display(Name = "最大温差"), PointIndex(34)]
        public int MaxTemperatureDifference { get; set; }

        [Display(Name = "最高单体温度"), PointIndex(35)]
        public int HighestSingleCellTemperature { get; set; }

        [Display(Name = "最高单体温度从机号"), PointIndex(36)]
        public int HighestSingleCellTemperatureSlaveId { get; set; }

        [Display(Name = "最高单体温度编号"), PointIndex(37)]
        public int HighestSingleCellTemperatureId { get; set; }

        [Display(Name = "最低单体温度"), PointIndex(38)]
        public int LowestSingleCellTemperature { get; set; }

        [Display(Name = "最低单体温度从机号"), PointIndex(39)]
        public int LowestSingleCellTemperatureSlaveId { get; set; }

        [Display(Name = "最低单体温度编号"), PointIndex(40)]
        public int LowestSingleCellTemperatureId { get; set; }

        [Display(Name = "日充电容量"), PointIndex(41)]
        public int DailyChargeCapacity { get; set; }

        [Display(Name = "日充电电量"), PointIndex(42)]
        public int DailyChargeEnergy { get; set; }

        [Display(Name = "日放电容量"), PointIndex(43)]
        public int DailyDischargeCapacity { get; set; }

        [Display(Name = "日放电电量"), PointIndex(44)]
        public int DailyDischargeEnergy { get; set; }

        [Display(Name = "日充电时间"), PointIndex(45)]
        public int DailyChargeTime { get; set; }

        [Display(Name = "日放电时间"), PointIndex(46)]
        public int DailyDischargeTime { get; set; }

        [Display(Name = "累计充电容量"), PointIndex(47)]
        public int TotalChargeCapacity { get; set; }

        [Display(Name = "累计充电电量"), PointIndex(48)]
        public int TotalChargeEnergy { get; set; }

        [Display(Name = "累计放电容量"), PointIndex(49)]
        public int TotalDischargeCapacity { get; set; }

        [Display(Name = "累计放电电量"), PointIndex(50)]
        public int TotalDischargeEnergy { get; set; }
        [Display(AutoGenerateField = false), PointRange(51, 99)]
        public int[] Reserved { get; set; }

    }
}
