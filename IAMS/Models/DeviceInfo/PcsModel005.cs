using IAMS.AttributeTag;
using System.ComponentModel.DataAnnotations;

namespace IAMS.Models.DeviceInfo {
    public class PcsModel005 : DeviceBaseInfo {
        [Display(Name = "是否在线"), PointIndex(0)]
        public bool OnlineStatus { get; set; }

        [Display(Name = "预留"), PointIndex(1)]
        public int Reserved1 { get; set; }

        [Display(Name = "总故障"), PointIndex(2)]
        public int TotalFault { get; set; }

        [Display(Name = "总告警"), PointIndex(3)]
        public int TotalAlarm { get; set; }

        [Display(Name = "A相硬件过流"), PointIndex(4)]
        public int PhaseAHardwareOvercurrent { get; set; }

        [Display(Name = "B相硬件过流"), PointIndex(5)]
        public int PhaseBHardwareOvercurrent { get; set; }

        [Display(Name = "C相硬件过流"), PointIndex(6)]
        public int PhaseCHardwareOvercurrent { get; set; }

        [Display(Name = "N相硬件过流"), PointIndex(7)]
        public int PhaseNHardwareOvercurrent { get; set; }

        [Display(Name = "单元直压"), PointIndex(8)]
        public int UnitDirectVoltage { get; set; }

        [Display(Name = "开关电源欠压"), PointIndex(9)]
        public int SwitchingPowerUndervoltage { get; set; }

        [Display(Name = "A相IGBT故障"), PointIndex(10)]
        public int PhaseAIGBTFault { get; set; }

        [Display(Name = "B相IGBT故障"), PointIndex(11)]
        public int PhaseBIGBTFault { get; set; }

        [Display(Name = "C相IGBT故障"), PointIndex(12)]
        public int PhaseCIGBTFault { get; set; }

        [Display(Name = "N相IGBT故障"), PointIndex(13)]
        public int PhaseNIGBTFault { get; set; }

        [Display(Name = "过温故障"), PointIndex(14)]
        public int OverTemperatureFault { get; set; }

        [Display(Name = "A相输出过流"), PointIndex(15)]
        public int PhaseAOutputOvercurrent { get; set; }

        [Display(Name = "A相输出速断"), PointIndex(16)]
        public int PhaseAOutputInstantaneousTrip { get; set; }

        [Display(Name = "B相输出过流"), PointIndex(17)]
        public int PhaseBOutputOvercurrent { get; set; }

        [Display(Name = "B相输出速断"), PointIndex(18)]
        public int PhaseBOutputInstantaneousTrip { get; set; }

        [Display(Name = "C相输出过流"), PointIndex(19)]
        public int PhaseCOutputOvercurrent { get; set; }

        [Display(Name = "C相输出速断"), PointIndex(20)]
        public int PhaseCOutputInstantaneousTrip { get; set; }

        [Display(Name = "N相输出过流"), PointIndex(21)]
        public int PhaseNOutputOvercurrent { get; set; }

        [Display(Name = "N相输出速断"), PointIndex(22)]
        public int PhaseNOutputInstantaneousTrip { get; set; }

        [Display(Name = "交流过压"), PointIndex(23)]
        public int ACOvervoltage { get; set; }

        [Display(Name = "交流欠压"), PointIndex(24)]
        public int ACUndervoltage { get; set; }

        [Display(Name = "交流过频"), PointIndex(25)]
        public int ACOverfrequency { get; set; }

        [Display(Name = "交流欠频"), PointIndex(26)]
        public int ACUnderfrequency { get; set; }

        [Display(Name = "电压THDU超限"), PointIndex(27)]
        public int VoltageTHDULimitExceeded { get; set; }

        [Display(Name = "系统缺相"), PointIndex(28)]
        public int SystemPhaseLoss { get; set; }

        [Display(Name = "系统相序错误"), PointIndex(29)]
        public int SystemPhaseSequenceError { get; set; }

        [Display(Name = "直流极性反接"), PointIndex(30)]
        public int DCPoleReversal { get; set; }

        [Display(Name = "直流母线欠压"), PointIndex(31)]
        public int DCBusUndervoltage { get; set; }

        [Display(Name = "直流母线过压"), PointIndex(32)]
        public int DCBusOvervoltage { get; set; }

        [Display(Name = "系统过频"), PointIndex(33)]
        public int SystemOverfrequency { get; set; }

        [Display(Name = "系统欠频"), PointIndex(34)]
        public int SystemUnderfrequency { get; set; }

        [Display(Name = "直流充电过流"), PointIndex(35)]
        public int DCChargeOvercurrent { get; set; }

        [Display(Name = "直流放电过流"), PointIndex(36)]
        public int DCDischargeOvercurrent { get; set; }

        [Display(Name = "孤岛保护"), PointIndex(37)]
        public int IslandingProtection { get; set; }

        [Display(AutoGenerateField = false), PointRange(38, 143)]
        public int[] ReservedFaultAndStatusBits { get; set; }

        [Display(Name = "停机状态"), PointIndex(144)]
        public int ShutdownStatus { get; set; }

        [Display(Name = "待机状态"), PointIndex(145)]
        public int StandbyStatus { get; set; }

        [Display(Name = "运行状态"), PointIndex(146)]
        public int RunningStatus { get; set; }

        [Display(Name = "并网状态"), PointIndex(147)]
        public int GridConnectedStatus { get; set; }

        [Display(Name = "离网状态"), PointIndex(148)]
        public int OffGridStatus { get; set; }

        [Display(Name = "BMS干节点信号"), PointIndex(149)]
        public int BMSDryNodeSignal { get; set; }

        [Display(Name = "本地/远程"), PointIndex(150)]
        public int LocalOrRemote { get; set; }

        [Display(Name = "A相电压"), PointIndex(151)]
        public int PhaseAVoltage { get; set; }

        [Display(Name = "B相电压"), PointIndex(152)]
        public int PhaseBVoltage { get; set; }

        [Display(Name = "C相电压"), PointIndex(153)]
        public int PhaseCVoltage { get; set; }

        [Display(Name = "A相电流"), PointIndex(154)]
        public int PhaseACurrent { get; set; }

        [Display(Name = "B相电流"), PointIndex(155)]
        public int PhaseBCurrent { get; set; }

        [Display(Name = "C相电流"), PointIndex(156)]
        public int PhaseCCurrent { get; set; }

        [Display(Name = "电网频率"), PointIndex(157)]
        public int GridFrequency { get; set; }

        [Display(Name = "A相有功功率"), PointIndex(158)]
        public int PhaseAActivePower { get; set; }

        [Display(Name = "B相有功功率"), PointIndex(159)]
        public int PhaseBActivePower { get; set; }

        [Display(Name = "C相有功功率"), PointIndex(160)]
        public int PhaseCActivePower { get; set; }

        [Display(Name = "总有功功率"), PointIndex(161)]
        public int TotalActivePower { get; set; }

        [Display(Name = "A相无功功率"), PointIndex(162)]
        public int PhaseAReactivePower { get; set; }

        [Display(Name = "B相无功功率"), PointIndex(163)]
        public int PhaseBReactivePower { get; set; }

        [Display(Name = "C相无功功率"), PointIndex(164)]
        public int PhaseCReactivePower { get; set; }

        [Display(Name = "总无功功率"), PointIndex(165)]
        public int TotalReactivePower { get; set; }

        [Display(Name = "A相视在功率"), PointIndex(166)]
        public int PhaseAApparentPower { get; set; }

        [Display(Name = "B相视在功率"), PointIndex(167)]
        public int PhaseBApparentPower { get; set; }

        [Display(Name = "C相视在功率"), PointIndex(168)]
        public int PhaseCApparentPower { get; set; }

        [Display(Name = "总视在功率"), PointIndex(169)]
        public int TotalApparentPower { get; set; }

        [Display(Name = "A相功率因数"), PointIndex(170)]
        public int PhaseAPowerFactor { get; set; }

        [Display(Name = "B相功率因数"), PointIndex(171)]
        public int PhaseBPowerFactor { get; set; }

        [Display(Name = "C相功率因数"), PointIndex(172)]
        public int PhaseCPowerFactor { get; set; }

        [Display(Name = "总功率因数"), PointIndex(173)]
        public int TotalPowerFactor { get; set; }

        [Display(Name = "PCS输入功率"), PointIndex(174)]
        public int PCSInputPower { get; set; }

        [Display(Name = "PCS输入电压"), PointIndex(175)]
        public int PCSInputVoltage { get; set; }

        [Display(Name = "PCS输入电流"), PointIndex(176)]
        public int PCSInputCurrent { get; set; }

        [Display(Name = "日充电量"), PointIndex(177)]
        public int DailyChargeAmount { get; set; }

        [Display(Name = "日放电量"), PointIndex(178)]
        public int DailyDischargeAmount { get; set; }

        [Display(Name = "交流累计充电量"), PointIndex(179)]
        public int ACAccumulatedChargeAmount { get; set; }

        [Display(Name = "交流累计放电量"), PointIndex(180)]
        public int ACAccumulatedDischargeAmount { get; set; }

        [Display(Name = "直流累计充电量"), PointIndex(181)]
        public int DCAccumulatedChargeAmount { get; set; }

        [Display(Name = "直流累计放电量"), PointIndex(182)]
        public int DCAccumulatedDischargeAmount { get; set; }

        [Display(Name = "最大允许充电电流"), PointIndex(183)]
        public int MaxAllowedChargeCurrent { get; set; }

        [Display(Name = "最大允许放电电流"), PointIndex(184)]
        public int MaxAllowedDischargeCurrent { get; set; }

        [Display(Name = "最大允许充电功率"), PointIndex(185)]
        public int MaxAllowedChargePower { get; set; }

        [Display(Name = "最大允许放电功率"), PointIndex(186)]
        public int MaxAllowedDischargePower { get; set; }

        [Display(Name = "IGBT温度"), PointIndex(187)]
        public int IGBTTemperature { get; set; }

        [Display(Name = "PCS故障字1"), PointIndex(188)]
        public int PCSFaultWord1 { get; set; }

        [Display(Name = "PCS故障字2"), PointIndex(189)]
        public int PCSFaultWord2 { get; set; }

        [Display(Name = "PCS故障字3"), PointIndex(190)]
        public int PCSFaultWord3 { get; set; }

        [Display(Name = "PCS故障字4"), PointIndex(191)]
        public int PCSFaultWord4 { get; set; }

        [Display(Name = "PCS故障字5"), PointIndex(192)]
        public int PCSFaultWord5 { get; set; }

        [Display(Name = "PCS故障字6"), PointIndex(193)]
        public int PCSFaultWord6 { get; set; }

        [Display(Name = "系统时钟-秒"), PointIndex(194)]
        public int SystemClockSecond { get; set; }

        [Display(Name = "系统时钟-分"), PointIndex(195)]
        public int SystemClockMinute { get; set; }

        [Display(Name = "系统时钟-时"), PointIndex(196)]
        public int SystemClockHour { get; set; }

        [Display(Name = "系统时钟-日"), PointIndex(197)]
        public int SystemClockDay { get; set; }

        [Display(Name = "系统时钟-月"), PointIndex(198)]
        public int SystemClockMonth { get; set; }

        [Display(Name = "系统时钟-年"), PointIndex(199)]
        public int SystemClockYear { get; set; }

        [Display(Name = "预留2"), PointRange(200,250)]
        public int[] Reserved2 { get; set; }

        [Display(Name = "远程本地设置"), PointIndex(251)]
        public int RemoteLocalSetting { get; set; }

        [Display(Name = "运行模式设置"), PointIndex(252)]
        public int OperationModeSetting { get; set; }

        [Display(Name = "并网离网设置"), PointIndex(253)]
        public int GridConnectionSetting { get; set; }

        [Display(Name = "设备开机"), PointIndex(254)]
        public int DevicePowerOn { get; set; }

        [Display(Name = "设备停机"), PointIndex(255)]
        public int DeviceShutdown { get; set; }

        [Display(Name = "恒功率有功功率设置"), PointIndex(257)]
        public int ConstantPowerActivePowerSetting { get; set; }

        [Display(Name = "恒功率无功功率设置"), PointIndex(258)]
        public int ConstantPowerReactivePowerSetting { get; set; }

        [Display(Name = "功率因数控制"), PointIndex(259)]
        public int PowerFactorControl { get; set; }

        [Display(AutoGenerateField = false), PointRange(259, 299)]
        public int[] ReservedRemoteAdjustmentAndControl { get; set; }



    }
}
