using IAMS.AttributeTag;
using System.ComponentModel.DataAnnotations;

namespace IAMS.Models.DeviceInfo {
    public class BcuModel004 : DeviceBaseInfo {
        [Display(Name = "设备启用"), PointIndex(0)]
        public bool DeviceEnable { get; set; }//设备启用0
        [Display(Name = "设备在线"), PointIndex(1)]
        public bool DeviceOnline { get; set; }//设备在线1
        [Display(Name = "总告警"), PointIndex(2)]
        public int TotalAlarm { get; set; }//总告警2
        [Display(Name = "总故障"), PointIndex(3)]
        public int TotalFault { get; set; }//总故障3
        [Display(AutoGenerateField = false, Name = "单体过压告警一级"), PointIndex(4), Alarm(1)]
        public int CellOvervoltageAlarmLevel1 { get; set; }//单体过压告警一级4
        [Display(AutoGenerateField = false, Name = "单体欠压告警一级"), PointIndex(5), Alarm(1)]
        public int CellUndervoltageAlarmLevel1 { get; set; }//单体欠压告警一级5
        [Display(AutoGenerateField = false, Name = "单体过温告警一级"), PointIndex(6), Alarm(1)]
        public int CellOvertemperatureAlarmLevel1 { get; set; }//单体过温告警一级6
        [Display(AutoGenerateField = false, Name = "单体低温告警一级"), PointIndex(7), Alarm(1)]
        public int CellLowTemperatureAlarmLevel1 { get; set; }//单体低温告警一级7
        [Display(AutoGenerateField = false, Name = "单体压差告警一级"), PointIndex(8), Alarm(1)]
        public int CellVoltageDifferenceAlarmLevel1 { get; set; }//单体压差告警一级8
        [Display(AutoGenerateField = false, Name = "充电过流告警一级"), PointIndex(9), Alarm(1)]
        public int ChargeOvercurrentAlarmLevel1 { get; set; }//充电过流告警一级9
        [Display(AutoGenerateField = false, Name = "放电过流告警一级"), PointIndex(10), Alarm(1)]
        public int DischargeOvercurrentAlarmLevel1 { get; set; }//放电过流告警一级10
        [Display(AutoGenerateField = false, Name = "SOC过低告警一级"), PointIndex(11), Alarm(1)]
        public int SocLowAlarmLevel1 { get; set; }//SOC过低告警一级11
        [Display(AutoGenerateField = false, Name = "SOC差异过大告警一级"), PointIndex(12), Alarm(1)]
        public int SocDifferenceTooLargeAlarmLevel1 { get; set; }//SOC差异过大告警一级12
        [Display(AutoGenerateField = false, Name = "绝缘过低告警一级"), PointIndex(13), Alarm(1)]
        public int InsulationLowAlarmLevel1 { get; set; }//绝缘过低告警一级13
        [Display(AutoGenerateField = false, Name = "单体过压告警二级"), PointIndex(14), Alarm(2)]
        public int CellOvervoltageAlarmLevel2 { get; set; }//单体过压告警二级14
        [Display(AutoGenerateField = false, Name = "单体欠压告警二级"), PointIndex(15), Alarm(2)]
        public int CellUndervoltageAlarmLevel2 { get; set; }//单体欠压告警二级15
        [Display(AutoGenerateField = false, Name = "单体过温告警二级"), PointIndex(16), Alarm(2)]
        public int CellOvertemperatureAlarmLevel2 { get; set; }//单体过温告警二级16
        [Display(AutoGenerateField = false, Name = "单体低温告警二级"), PointIndex(17), Alarm(2)]
        public int CellLowTemperatureAlarmLevel2 { get; set; }//单体低温告警二级17
        [Display(AutoGenerateField = false, Name = "单体压差告警二级"), PointIndex(18), Alarm(2)]
        public int CellVoltageDifferenceAlarmLevel2 { get; set; }//单体压差告警二级18
        [Display(AutoGenerateField = false, Name = "充电过流告警二级"), PointIndex(19), Alarm(2)]
        public int ChargeOvercurrentAlarmLevel2 { get; set; }//充电过流告警二级19
        [Display(AutoGenerateField = false, Name = "放电过流告警二级"), PointIndex(20), Alarm(2)]
        public int DischargeOvercurrentAlarmLevel2 { get; set; }//放电过流告警二级20
        [Display(AutoGenerateField = false, Name = "SOC过低告警二级"), PointIndex(21), Alarm(2)]
        public int SocLowAlarmLevel2 { get; set; }//SOC过低告警二级21
        [Display(AutoGenerateField = false, Name = "SOC差异过大告警二级"), PointIndex(22), Alarm(2)]
        public int SocDifferenceTooLargeAlarmLevel2 { get; set; }//SOC差异过大告警二级22
        [Display(AutoGenerateField = false, Name = "绝缘过低告警二级"), PointIndex(23), Alarm(2)]
        public int InsulationLowAlarmLevel2 { get; set; }//绝缘过低告警二级23
        [Display(AutoGenerateField = false, Name = "单体过压告警三级"), PointIndex(24), Alarm(3)]
        public int CellOvervoltageAlarmLevel3 { get; set; }//单体过压告警三级24
        [Display(AutoGenerateField = false, Name = "单体欠压告警三级"), PointIndex(25), Alarm(3)]
        public int CellUndervoltageAlarmLevel3 { get; set; }//单体欠压告警三级25
        [Display(AutoGenerateField = false, Name = "单体过温告警三级"), PointIndex(26), Alarm(3)]
        public int CellOvertemperatureAlarmLevel3 { get; set; }//单体过温告警三级26
        [Display(AutoGenerateField = false, Name = "单体低温告警三级"), PointIndex(27), Alarm(3)]
        public int CellLowTemperatureAlarmLevel3 { get; set; }//单体低温告警三级27
        [Display(AutoGenerateField = false, Name = "单体压差告警三级"), PointIndex(28), Alarm(3)]
        public int CellVoltageDifferenceAlarmLevel3 { get; set; }//单体压差告警三级28
        [Display(AutoGenerateField = false, Name = "充电过流告警三级"), PointIndex(29), Alarm(3)]
        public int ChargeOvercurrentAlarmLevel3 { get; set; }//充电过流告警三级29
        [Display(AutoGenerateField = false, Name = "放电过流告警三级"), PointIndex(30), Alarm(3)]
        public int DischargeOvercurrentAlarmLevel3 { get; set; }//放电过流告警三级30
        [Display(AutoGenerateField = false, Name = "SOC过低告警三级"), PointIndex(31), Alarm(3)]
        public int SocLowAlarmLevel3 { get; set; }//SOC过低告警三级31
        [Display(AutoGenerateField = false, Name = "SOC差异过大告警三级"), PointIndex(32), Alarm(3)]
        public int SocDifferenceTooLargeAlarmLevel3 { get; set; }//SOC差异过大告警三级32
        [Display(AutoGenerateField = false, Name = "绝缘过低告警三级"), PointIndex(33), Alarm(3)]
        public int InsulationLowAlarmLevel3 { get; set; }//绝缘过低告警三级33
        [Display(AutoGenerateField = false, Name = "电芯温度极限告警"), PointIndex(34), Alarm(4)]
        public int CellTemperatureLimitAlarm { get; set; }//电芯温度极限告警34
        [Display(AutoGenerateField = false, Name = "电芯电压极限告警"), PointIndex(35), Alarm(4)]
        public int CellVoltageLimitAlarm { get; set; }//电芯电压极限告警35
        [Display(AutoGenerateField = false, Name = "簇间环流1级告警"), PointIndex(36), Alarm(1)]
        public int InterClusterCirculationAlarmLevel1 { get; set; }//簇间环流1级告警36
        [Display(AutoGenerateField = false, Name = "簇间环流2级告警"), PointIndex(37), Alarm(2)]
        public int InterClusterCirculationAlarmLevel2 { get; set; }//簇间环流2级告警37
        [Display(AutoGenerateField = false, Name = "簇间环流3级告警"), PointIndex(38), Alarm(3)]
        public int InterClusterCirculationAlarmLevel3 { get; set; }//簇间环流3级告警38
        [Display(AutoGenerateField = false, Name = "簇间电流差1级告警"), PointIndex(39), Alarm(1)]
        public int InterClusterCurrentDifferenceAlarmLevel1 { get; set; }//簇间电流差1级告警39
        [Display(AutoGenerateField = false, Name = "簇间电流差2级告警"), PointIndex(40), Alarm(2)]
        public int InterClusterCurrentDifferenceAlarmLevel2 { get; set; }//簇间电流差2级告警40
        [Display(AutoGenerateField = false, Name = "簇间电流差3级告警"), PointIndex(41), Alarm(3)]
        public int InterClusterCurrentDifferenceAlarmLevel3 { get; set; }//簇间电流差3级告警41
        [Display(AutoGenerateField = false, Name = "组端过压1级告警"), PointIndex(42), Alarm(1)]
        public int GroupTerminalOvervoltageAlarmLevel1 { get; set; }//组端过压1级告警42
        [Display(AutoGenerateField = false, Name = "组端过压2级告警"), PointIndex(43), Alarm(2)]
        public int GroupTerminalOvervoltageAlarmLevel2 { get; set; }//组端过压2级告警43
        [Display(AutoGenerateField = false, Name = "组端过压3级告警"), PointIndex(44), Alarm(3)]
        public int GroupTerminalOvervoltageAlarmLevel3 { get; set; }//组端过压3级告警44
        [Display(AutoGenerateField = false, Name = "组端欠压1级告警"), PointIndex(45), Alarm(1)]
        public int GroupTerminalUndervoltageAlarmLevel1 { get; set; }//组端欠压1级告警45
        [Display(AutoGenerateField = false, Name = "组端欠压2级告警"), PointIndex(46), Alarm(1)]
        public int GroupTerminalUndervoltageAlarmLevel2 { get; set; }//组端欠压2级告警46
        [Display(AutoGenerateField = false, Name = "组端欠压3级告警"), PointIndex(47), Alarm(3)]
        public int GroupTerminalUndervoltageAlarmLevel3 { get; set; }//组端欠压3级告警47
        [Display(AutoGenerateField = false, Name = "极柱过温1级告警"), PointIndex(48), Alarm(1)]
        public int PoleOvertemperatureAlarmLevel1 { get; set; }//极柱过温1级告警48
        [Display(AutoGenerateField = false, Name = "极柱过温2级告警"), PointIndex(49), Alarm(2)]
        public int PoleOvertemperatureAlarmLevel2 { get; set; }//极柱过温2级告警49
        [Display(AutoGenerateField = false, Name = "极柱过温3级告警"), PointIndex(50), Alarm(3)]
        public int PoleOvertemperatureAlarmLevel3 { get; set; }//极柱过温3级告警50
        [Display(AutoGenerateField = false, Name = "AFE温感排线异常"), PointIndex(51), Alarm(4)]
        public int AfeTemperatureSensorCableAbnormal { get; set; }//AFE温感排线异常51
        [Display(AutoGenerateField = false, Name = "AFE电压排线异常"), PointIndex(52), Alarm(4)]
        public int AfeVoltageCableAbnormal { get; set; }//AFE电压排线异常52
        [Display(AutoGenerateField = false, Name = "与电池簇通信告警"), PointIndex(53), Alarm(4)]
        public int BatteryClusterCommunicationAlarm { get; set; }//与电池簇通信告警53
        [Display(AutoGenerateField = false, Name = "主从通讯告警"), PointIndex(54), Alarm(4)]
        public int MasterSlaveCommunicationAlarm { get; set; }//主从通讯告警54
        [Display(AutoGenerateField = false, Name = "继电器粘连告警"), PointIndex(65), Alarm(4)]
        public int RelayStickingAlarm { get; set; }//继电器粘连告警65
        [Display(AutoGenerateField = false, Name = "电池极限故障"), PointIndex(66)]
        public int BatteryLimitFault { get; set; }//电池极限故障66
        [Display(AutoGenerateField = false, Name = "熔丝故障"), PointIndex(55)]
        public int FuseFault { get; set; }//熔丝故障55
        [Display(AutoGenerateField = false, Name = "断路器故障"), PointIndex(56)]
        public int CircuitBreakerFault { get; set; }//断路器故障56
        [Display(AutoGenerateField = false, Name = "空调故障"), PointIndex(57)]
        public int AirConditionerFault { get; set; }//空调故障57
        [Display(AutoGenerateField = false, Name = "消防设备故障"), PointIndex(58)]
        public int FirefightingEquipmentFault { get; set; }//消防设备故障58
        [Display(AutoGenerateField = false, Name = "消防火警"), PointIndex(59)]
        public int FireAlarm { get; set; }//消防火警59
        [Display(AutoGenerateField = false, Name = "消防喷洒"), PointIndex(60)]
        public int FireSprinkler { get; set; }//消防喷洒60
        [Display(AutoGenerateField = false, Name = "AFE故障"), PointIndex(61)]
        public int AfeFault { get; set; }//AFE故障61
        [Display(AutoGenerateField = false, Name = "高压异常"), PointIndex(62)]
        public int HighVoltageAbnormal { get; set; }//高压异常62
        [Display(AutoGenerateField = false, Name = "预充告警"), PointIndex(63)]
        public int PreChargeAlarm { get; set; }//预充告警63
        [Display(AutoGenerateField = false, Name = "开路故障"), PointIndex(64)]
        public int OpenCircuitFault { get; set; }//开路故障64
        [Display(AutoGenerateField = false), PointIndex(67)]
        public int PrechargeRelayStatus { get; set; }//预充继电器状态
        [Display(AutoGenerateField = false), PointIndex(68)]
        public int MainPositiveRelayStatus { get; set; }//总正继电器状态
        [Display(AutoGenerateField = false), PointIndex(69)]
        public int MainNegativeRelayStatus { get; set; }//总负继电器状态
        [Display(AutoGenerateField = false), PointRange(70, 100)]
        public int[] AlarmStatusReserved { get; set; } //告警+状态保留继续~100
        [Display(Name = "总压"), PointIndex(101)]
        public float TotalVoltage { get; set; }//总压101
        [Display(Name = "总电流"), PointIndex(102)]
        public float TotalCurrent { get; set; }//总电流102
        [Display(Name = "SOC"), PointIndex(103)]
        public int Soc { get; set; }//SOC103
        [Display(Name = "SOH"), PointIndex(104)]
        public int Soh { get; set; }//SOH104
        [Display(Name = "SOE"), PointIndex(105)]
        public int Soe { get; set; }//SOE105
        [Display(Name = "额定总压"), PointIndex(106)]
        public float RatedTotalVoltage { get; set; }//额定总压106
        [Display(Name = "额定容量"), PointIndex(107)]
        public float RatedCapacity { get; set; }//额定容量107
        [Display(Name = "剩余容量"), PointIndex(108)]
        public float RemainingCapacity { get; set; }//剩余容量108
        [Display(Name = "额定电量"), PointIndex(109)]
        public float RatedEnergy { get; set; }//额定电量109
        [Display(Name = "剩余电量"), PointIndex(110)]
        public float RemainingEnergy { get; set; }//剩余电量110
        [Display(Name = "从机总数(BMU)"), PointIndex(111)]
        public int TotalSlavesBmu { get; set; }//从机总数(BMU)111
        [Display(Name = "在线从机总数(BMU)"), PointIndex(112)]
        public int OnlineSlavesBmu { get; set; }//在线从机总数(BMU)112
        [Display(Name = "电池总数"), PointIndex(113)]
        public int TotalBatteries { get; set; }//电池总数113
        [Display(Name = "在线电池总数"), PointIndex(114)]
        public int OnlineBatteries { get; set; }//在线电池总数114
        [Display(Name = "温感总数"), PointIndex(115)]
        public int TotalTemperatureSensors { get; set; }//温感总数115
        [Display(Name = "在线温感总数"), PointIndex(116)]
        public int OnlineTemperatureSensors { get; set; }//在线温感总数116
        [Display(Name = "最大允许放电电流"), PointIndex(117)]
        public float MaxAllowableDischargeCurrent { get; set; }//最大允许放电电流117
        [Display(Name = "最大允许放电功率"), PointIndex(118)]
        public float MaxAllowableDischargePower { get; set; }//最大允许放电功率118
        [Display(Name = "最大允许充电电流"), PointIndex(119)]
        public float MaxAllowableChargeCurrent { get; set; }//最大允许充电电流119
        [Display(Name = "最大允许充电功率"), PointIndex(120)]
        public float MaxAllowableChargePower { get; set; }//最大允许充电功率120
        [Display(Name = "正极绝缘阻值"), PointIndex(121)]
        public int PositiveInsulationResistance { get; set; }//正极绝缘阻值121
        [Display(Name = "负极绝缘阻值"), PointIndex(122)]
        public int NegativeInsulationResistance { get; set; }//负极绝缘阻值122
        [Display(Name = "单体平均电压"), PointIndex(123)]
        public int CellAverageVoltage { get; set; }//单体平均电压123
        [Display(Name = "单体最大压差"), PointIndex(124)]
        public int CellMaxVoltageDifference { get; set; }//单体最大压差124
        [Display(Name = "最高单体电压"), PointIndex(125)]
        public int HighestCellVoltage { get; set; }//最高单体电压125
        [Display(Name = "最高单体电压从机号"), PointIndex(126)]
        public int HighestCellVoltageSlaveId { get; set; }//最高单体电压从机号126
        [Display(Name = "最高单体电压编号"), PointIndex(127)]
        public int HighestCellVoltageId { get; set; }//最高单体电压编号127
        [Display(Name = "最低单体电压"), PointIndex(128)]
        public int LowestCellVoltage { get; set; }//最低单体电压128
        [Display(Name = "最低单体电压从机号"), PointIndex(129)]
        public int LowestCellVoltageSlaveId { get; set; }//最低单体电压从机号129
        [Display(Name = "最低单体电压编号"), PointIndex(130)]
        public int LowestCellVoltageId { get; set; }//最低单体电压编号130
        [Display(Name = "单体平均温度"), PointIndex(131)]
        public int CellAverageTemperature { get; set; }//单体平均温度131
        [Display(Name = "最大温差"), PointIndex(132)]
        public int MaxTemperatureDifference { get; set; }//最大温差132
        [Display(Name = "最高单体温度"), PointIndex(133)]
        public int HighestCellTemperature { get; set; }//最高单体温度133
        [Display(Name = "最高单体温度从机号"), PointIndex(134)]
        public int HighestCellTemperatureSlaveId { get; set; }//最高单体温度从机号134
        [Display(Name = "最高单体温度编号"), PointIndex(135)]
        public int HighestCellTemperatureId { get; set; }//最高单体温度编号135
        [Display(Name = "最低单体温度"), PointIndex(136)]
        public int LowestCellTemperature { get; set; }//最低单体温度136
        [Display(Name = "最低单体温度从机号"), PointIndex(137)]
        public int LowestCellTemperatureSlaveId { get; set; }//最低单体温度从机号137
        [Display(Name = "最低单体温度编号"), PointIndex(138)]
        public int LowestCellTemperatureId { get; set; }//最低单体温度编号138
        [Display(Name = "日充电容量"), PointIndex(139)]
        public float DailyChargeCapacity { get; set; }//日充电容量139
        [Display(Name = "日充电电量"), PointIndex(140)]
        public float DailyChargeEnergy { get; set; }//日充电电量140
        [Display(Name = "日放电容量"), PointIndex(141)]
        public float DailyDischargeCapacity { get; set; }//日放电容量141
        [Display(Name = "日放电电量"), PointIndex(142)]
        public float DailyDischargeEnergy { get; set; }//日放电电量142
        [Display(Name = "日充电时间"), PointIndex(143)]
        public float DailyChargeTime { get; set; }//日充电时间143
        [Display(Name = "日放电时间"), PointIndex(144)]
        public float DailyDischargeTime { get; set; }//日放电时间144
        [Display(Name = "累计充电容量"), PointIndex(145)]
        public float CumulativeChargeCapacity { get; set; }//累计充电容量145
        [Display(Name = "累计充电电量"), PointIndex(146)]
        public float CumulativeChargeEnergy { get; set; }//累计充电电量146
        [Display(Name = "累计放电容量"), PointIndex(147)]
        public float CumulativeDischargeCapacity { get; set; }//累计放电容量147
        [Display(Name = "累计放电电量"), PointIndex(148)]
        public float CumulativeDischargeEnergy { get; set; }//累计放电电量148
        [Display(Name = "累计充电时间"), PointIndex(149)]
        public float CumulativeChargeTime { get; set; }//累计充电时间149
        [Display(Name = "累计放电时间"), PointIndex(150)]
        public int CumulativeDischargeTime { get; set; }//累计放电时间150
        [Display(Name = "BCU工作状态"), PointIndex(151)]
        public int BcuOperatingStatus { get; set; }//BCU工作状态151
        [Display(AutoGenerateField = false), PointIndex(152)]
        public int FaultWord1 { get; set; }//故障字1152
        [Display(AutoGenerateField = false), PointIndex(153)]
        public int FaultWord2 { get; set; }//故障字2153
        [Display(AutoGenerateField = false), PointIndex(154)]
        public int FaultWord3 { get; set; }//故障字3154
        [Display(AutoGenerateField = false), PointIndex(155)]
        public int FaultWord4 { get; set; }//故障字4155
        [Display(AutoGenerateField = false), PointIndex(156)]
        public int FaultWord5 { get; set; }//故障字5156
        [Display(AutoGenerateField = false), PointIndex(157)]
        public int FaultWord6 { get; set; }//故障字6157
        [Display(AutoGenerateField = false), PointIndex(158)]
        public int FaultWord7 { get; set; }//故障字7158
        [Display(AutoGenerateField = false), PointIndex(159)]
        public int FaultWord8 { get; set; }//故障字8159
        [Display(AutoGenerateField = false), PointIndex(160)]
        public int FaultWord9 { get; set; }//故障字9160
        [Display(AutoGenerateField = false, Name = "1#PACK~30#PACK均衡状态"), PointRange(161, 190)]
        public int[] Pack1ToPack30BalancingStatus { get; set; }//1#PACK~30#PACK均衡状态161-190
        [Display(AutoGenerateField = false, Name = "1号~420号电池电压"), PointRange(191, 610)]
        public int[] BatteryVoltage1ToBattery420 { get; set; } //1号~420号电池电压191~610
        [Display(AutoGenerateField = false, Name = "1号~200号电池温度"), PointRange(611, 810)]
        public int[] BatteryTemperature1ToBattery200 { get; set; }//1号~200号电池温度611~810
        [Display(AutoGenerateField = false), PointRange(811, 1000)]
        public int[] Reserved { get; set; }//预留811~1000*/
        [Display(Name = "高压下电指令"), PointIndex(1001)]
        public int HighVoltagePowerOffCommand { get; set; }//高压下电指令1001
        [Display(AutoGenerateField = false), PointRange(1001, 1999)]
        public int[] ReservedThresholdAndParameters { get; set; } //预留阈值及参数1001~1999

    }
}
