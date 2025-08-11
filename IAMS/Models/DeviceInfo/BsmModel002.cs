using IAMS.AttributeTag;
using System.ComponentModel.DataAnnotations;

namespace IAMS.Models.DeviceInfo {
    public class BsmModel002: DeviceBaseInfo {
        [Display(Name = "是否在线"), PointIndex(0)]
        public int OnlineStatus { get; set; }

        [Display(Name = "A相电压"), PointIndex(1)]
        public int PhaseAVoltage { get; set; }

        [Display(Name = "B相电压"), PointIndex(2)]
        public int PhaseBVoltage { get; set; }

        [Display(Name = "C相电压"), PointIndex(3)]
        public int PhaseCVoltage { get; set; }

        [Display(Name = "AB线电压"), PointIndex(4)]
        public int LineVoltageAB { get; set; }

        [Display(Name = "BC线电压"), PointIndex(5)]
        public int LineVoltageBC { get; set; }

        [Display(Name = "CA线电压"), PointIndex(6)]
        public int LineVoltageCA { get; set; }

        [Display(Name = "A相电流"), PointIndex(7)]
        public int PhaseACurrent { get; set; }

        [Display(Name = "B相电流"), PointIndex(8)]
        public int PhaseBCurrent { get; set; }

        [Display(Name = "C相电流"), PointIndex(9)]
        public int PhaseCCurrent { get; set; }

        [Display(Name = "A相有功功率"), PointIndex(10)]
        public int PhaseAActivePower { get; set; }

        [Display(Name = "B相有功功率"), PointIndex(11)]
        public int PhaseBActivePower { get; set; }

        [Display(Name = "C相有功功率"), PointIndex(12)]
        public int PhaseCActivePower { get; set; }

        [Display(Name = "总有功功率"), PointIndex(13)]
        public int TotalActivePower { get; set; }

        [Display(Name = "A相无功功率"), PointIndex(14)]
        public int PhaseAReactivePower { get; set; }

        [Display(Name = "B相无功功率"), PointIndex(15)]
        public int PhaseBReactivePower { get; set; }

        [Display(Name = "C相无功功率"), PointIndex(16)]
        public int PhaseCReactivePower { get; set; }

        [Display(Name = "总无功功率"), PointIndex(17)]
        public int TotalReactivePower { get; set; }

        [Display(Name = "A相视在功率"), PointIndex(18)]
        public int PhaseAApparentPower { get; set; }

        [Display(Name = "B相视在功率"), PointIndex(19)]
        public int PhaseBApparentPower { get; set; }

        [Display(Name = "C相视在功率"), PointIndex(20)]
        public int PhaseCApparentPower { get; set; }

        [Display(Name = "总视在功率"), PointIndex(21)]
        public int TotalApparentPower { get; set; }

        [Display(Name = "A相功率因数"), PointIndex(22)]
        public int PhaseAPowerFactor { get; set; }

        [Display(Name = "B相功率因数"), PointIndex(23)]
        public int PhaseBPowerFactor { get; set; }

        [Display(Name = "C相功率因数"), PointIndex(24)]
        public int PhaseCPowerFactor { get; set; }

        [Display(Name = "总功率因数"), PointIndex(25)]
        public int TotalPowerFactor { get; set; }

        [Display(Name = "电网频率"), PointIndex(26)]
        public int GridFrequency { get; set; }

        [Display(Name = "电压互感比"), PointIndex(27)]
        public int VoltageTransformerRatio { get; set; }

        [Display(Name = "电流互感比"), PointIndex(28)]
        public int CurrentTransformerRatio { get; set; }

        [Display(Name = "正向有功电度"), PointIndex(29)]
        public int ForwardActiveEnergy { get; set; }

        [Display(Name = "反向有功电度"), PointIndex(30)]
        public int ReverseActiveEnergy { get; set; }

        [Display(Name = "正向无功电度"), PointIndex(31)]
        public int ForwardReactiveEnergy { get; set; }

        [Display(Name = "反向无功电度"), PointIndex(32)]
        public int ReverseReactiveEnergy { get; set; }

        [Display(Name = "尖正向有功电度"), PointIndex(33)]
        public int PeakForwardActiveEnergy { get; set; }

        [Display(Name = "尖反向有功电度"), PointIndex(34)]
        public int PeakReverseActiveEnergy { get; set; }

        [Display(Name = "峰正向有功电度"), PointIndex(35)]
        public int HighForwardActiveEnergy { get; set; }

        [Display(Name = "峰反向有功电度"), PointIndex(36)]
        public int HighReverseActiveEnergy { get; set; }

        [Display(Name = "平正向有功电度"), PointIndex(37)]
        public int FlatForwardActiveEnergy { get; set; }

        [Display(Name = "平反向有功电度"), PointIndex(38)]
        public int FlatReverseActiveEnergy { get; set; }

        [Display(Name = "谷正向有功电度"), PointIndex(39)]
        public int ValleyForwardActiveEnergy { get; set; }

        [Display(Name = "谷反向有功电度"), PointIndex(40)]
        public int ValleyReverseActiveEnergy { get; set; }

        [Display(Name = "当月正向有功最大需量"), PointIndex(41)]
        public int MaxForwardActiveDemandCurrentMonth { get; set; }

        [Display(Name = "当月反向有功最大需量"), PointIndex(42)]
        public int MaxReverseActiveDemandCurrentMonth { get; set; }

        [Display(AutoGenerateField = false), PointRange(43, 60)]
        public int[] Reserved { get; set; }

    }
}
