﻿namespace IAMS.Models.StationSystem {
    public class PCSInfo : DeviceDataBaseInfo {
        public bool IsEnabled { get; set; }
        public bool IsOnline { get; set; }
        public double TotalFault { get; set; } = 0; //	总故障	2
        public double TotalAlarm { get; set; } = 0; //	总告警	3
        public double HardwareOvercurrentPhaseA { get; set; } = 0;  //	A相硬件过流	4
        public double HardwareOvercurrentPhaseB { get; set; } = 0;  //	B相硬件过流	5
        public double HardwareOvercurrentPhaseC { get; set; } = 0;  //	C相硬件过流	6
        public double HardwareOvercurrentPhaseN { get; set; } = 0;  //	N相硬件过流	7
        public double UnitDCVoltage { get; set; } = 0;  //	单元直压	8
        public double SwitchPowerUnderVoltage { get; set; } = 0;    //	开关电源欠压	9
        public double IGBTFaultPhaseA { get; set; } = 0;    //	A相IGBT故障	10
        public double IGBTFaultPhaseB { get; set; } = 0;    //	B相IGBT故障	11
        public double IGBTFaultPhaseC { get; set; } = 0;    //	C相IGBT故障	12
        public double IGBTFaultPhaseN { get; set; } = 0;    //	N相IGBT故障	13
        public double OverTemperatureFault { get; set; } = 0;   //	过温故障	14
        public double OutputOvercurrentPhaseA { get; set; } = 0;    //	A相输出过流	15
        public double OutputShortCircuitPhaseA { get; set; } = 0;   //	A相输出速断	16
        public double OutputOvercurrentPhaseB { get; set; } = 0;    //	B相输出过流	17
        public double OutputShortCircuitPhaseB { get; set; } = 0;   //	B相输出速断	18
        public double OutputOvercurrentPhaseC { get; set; } = 0;    //	C相输出过流	19
        public double OutputShortCircuitPhaseC { get; set; } = 0;   //	C相输出速断	20
        public double OutputOvercurrentPhaseN { get; set; } = 0;    //	N相输出过流	21
        public double OutputShortCircuitPhaseN { get; set; } = 0;   //	N相输出速断	22
        public double ACOverVoltage { get; set; } = 0;  //	交流过压	23
        public double ACUnderVoltage { get; set; } = 0; //	交流欠压	24
        public double ACOverFrequency { get; set; } = 0;    //	交流过频	25
        public double ACUnderFrequency { get; set; } = 0;   //	交流欠频	26
        public double VoltageTHDUExceed { get; set; } = 0;  //	电压THDU超限	27
        public double SystemPhaseLoss { get; set; } = 0;    //	系统缺相	28
        public double SystemPhaseSequenceError { get; set; } = 0;   //	系统相序错误	29
        public double DCPolarityReverse { get; set; } = 0;  //	直流极性反接	30
        public double DCBusUnderVoltage { get; set; } = 0;  //	直流母线欠压	31
        public double DCBusOverVoltage { get; set; } = 0;   //	直流母线过压	32
        public double SystemOverFrequency { get; set; } = 0;    //	系统过频	33
        public double SystemUnderFrequency { get; set; } = 0;   //	系统欠频	34
        public double DCChargingOvercurrent { get; set; } = 0;  //	直流充电过流	35
        public double DCDischargingOvercurrent { get; set; } = 0;   //	直流放电过流	36
        public double IslandProtection { get; set; } = 0;   //	孤岛保护	37
        public double ReservedFaultAndStatus { get; set; } = 0; //	预留故障+状态位	150
        public double VoltagePhaseA { get; set; } = 0;  //	A相电压	151
        public double VoltagePhaseB { get; set; } = 0;  //	B相电压	152
        public double VoltagePhaseC { get; set; } = 0;  //	C相电压	153
        public double CurrentPhaseA { get; set; } = 0;  //	A相电流	154
        public double CurrentPhaseB { get; set; } = 0;  //	B相电流	155
        public double CurrentPhaseC { get; set; } = 0;  //	C相电流	156
        public double GridFrequency { get; set; } = 0;  //	电网频率	157
        public double ActivePowerPhaseA { get; set; } = 0;  //	A相有功功率	158
        public double ActivePowerPhaseB { get; set; } = 0;  //	B相有功功率	159
        public double ActivePowerPhaseC { get; set; } = 0;  //	C相有功功率	160
        public double TotalActivePower { get; set; } = 0;   //	总有功功率	161
        public double ReactivePowerPhaseA { get; set; } = 0;    //	A相无功功率	162
        public double ReactivePowerPhaseB { get; set; } = 0;    //	B相无功功率	163
        public double ReactivePowerPhaseC { get; set; } = 0;    //	C相无功功率	164
        public double TotalReactivePower { get; set; } = 0; //	总无功功率	165
        public double ApparentPowerPhaseA { get; set; } = 0;    //	A相视在功率	166
        public double ApparentPowerPhaseB { get; set; } = 0;    //	B相视在功率	167
        public double ApparentPowerPhaseC { get; set; } = 0;    //	C相视在功率	168
        public double TotalApparentPower { get; set; } = 0; //	总视在功率	169
        public double PowerFactorPhaseA { get; set; } = 0;  //	A相功率因数	170
        public double PowerFactorPhaseB { get; set; } = 0;  //	B相功率因数	171
        public double PowerFactorPhaseC { get; set; } = 0;  //	C相功率因数	172
        public double TotalPowerFactor { get; set; } = 0;   //	总功率因数	173
        public double PCSEntryPower { get; set; } = 0;  //	PCS输入功率	174
        public double PCSEntryVoltage { get; set; } = 0;    //	PCS输入电压	175
        public double PCSEntryCurrent { get; set; } = 0;    //	PCS输入电流	176
        public double DailyChargeAmount { get; set; } = 0;  //	日充电量	177
        public double DailyDischargeAmount { get; set; } = 0;   //	日放电量	178
        public double ACCumulativeChargeAmount { get; set; } = 0;   //	交流累计充电量	179
        public double ACCumulativeDischargeAmount { get; set; } = 0;    //	交流累计放电量	180
        public double DCCumulativeChargeAmount { get; set; } = 0;   //	直流累计充电量	181
        public double DCCumulativeDischargeAmount { get; set; } = 0;    //	直流累计放电量	182
        public double MaxAllowableChargeCurrent { get; set; } = 0;  //	最大允许充电电流	183
        public double MaxAllowableDischargeCurrent { get; set; } = 0;   //	最大允许放电电流	184
        public double MaxAllowableChargePower { get; set; } = 0;    //	最大允许充电功率	185
        public double MaxAllowableDischargePower { get; set; } = 0; //	最大允许放电功率	186
        public double IGBTTemperature { get; set; } = 0;    //	IGBT温度	187
        public double PCSFaultWord1 { get; set; } = 0;  //	PCS故障字1	188
        public double PCSFaultWord2 { get; set; } = 0;  //	PCS故障字2	189
        public double PCSFaultWord3 { get; set; } = 0;  //	PCS故障字3	190
        public double PCSFaultWord4 { get; set; } = 0;  //	PCS故障字4	191
        public double PCSFaultWord5 { get; set; } = 0;  //	PCS故障字5	192
        public double PCSFaultWord6 { get; set; } = 0;  //	PCS故障字6	193
        public double SystemClockSeconds { get; set; } = 0; //	系统时钟-秒	194
        public double SystemClockMinutes { get; set; } = 0; //	系统时钟-分	195
        public double SystemClockHours { get; set; } = 0;   //	系统时钟-时	196
        public double SystemClockDay { get; set; } = 0; //	系统时钟-日	197
        public double SystemClockMonth { get; set; } = 0;   //	系统时钟-月	198
        public double SystemClockYear { get; set; } = 0;    //	系统时钟-年	199
        public double Reserved2 { get; set; } = 0;  //	预留2	200-250
        public double RemoteLocalSetting { get; set; } = 0; //	远程本地设置	251
        public double OperatingModeSetting { get; set; } = 0;   //	运行模式设置	252
        public double GridConnectionDisconnectionSetting { get; set; } = 0; //	并网离网设置	253
        public double DevicePowerOn { get; set; } = 0;  //	设备开机	254
        public double DevicePowerOff { get; set; } = 0; //	设备停机	255
        public double ConstantPowerActivePowerSetting { get; set; } = 0;    //	设备开关机指令	256
        public double ConstantPowerReactivePowerSetting { get; set; } = 0;  //	恒功率有功功率设置	257
        public double PowerFactorControl { get; set; } = 0; //	恒功率无功功率设置	258
        public double ReservedRemoteAdjustmentAndControl { get; set; } = 0;	//	功率因数控制	259

        public Dictionary<string, string> Property2ChineseName = new Dictionary<string, string>() {
            {"IsEnabled","是否启用"},
            {"IsOnline","是否在线"},
            {"TotalFault","总故障"},
            {"TotalAlarm","总告警"},
            {"HardwareOvercurrentPhaseA","A相硬件过流"},
            {"HardwareOvercurrentPhaseB","B相硬件过流"},
            {"HardwareOvercurrentPhaseC","C相硬件过流"},
            {"HardwareOvercurrentPhaseN","N相硬件过流"},
            {"UnitDCVoltage","单元直压"},
            {"SwitchPowerUnderVoltage","开关电源欠压"},
            {"IGBTFaultPhaseA","A相IGBT故障"},
            {"IGBTFaultPhaseB","B相IGBT故障"},
            {"IGBTFaultPhaseC","C相IGBT故障"},
            {"IGBTFaultPhaseN","N相IGBT故障"},
            {"OverTemperatureFault","过温故障"},
            {"OutputOvercurrentPhaseA","A相输出过流"},
            {"OutputShortCircuitPhaseA","A相输出速断"},
            {"OutputOvercurrentPhaseB","B相输出过流"},
            {"OutputShortCircuitPhaseB","B相输出速断"},
            {"OutputOvercurrentPhaseC","C相输出过流"},
            {"OutputShortCircuitPhaseC","C相输出速断"},
            {"OutputOvercurrentPhaseN","N相输出过流"},
            {"OutputShortCircuitPhaseN","N相输出速断"},
            {"ACOverVoltage","交流过压"},
            {"ACUnderVoltage","交流欠压"},
            {"ACOverFrequency","交流过频"},
            {"ACUnderFrequency","交流欠频"},
            {"VoltageTHDUExceed","电压THDU超限"},
            {"SystemPhaseLoss","系统缺相"},
            {"SystemPhaseSequenceError","系统相序错误"},
            {"DCPolarityReverse","直流极性反接"},
            {"DCBusUnderVoltage","直流母线欠压"},
            {"DCBusOverVoltage","直流母线过压"},
            {"SystemOverFrequency","系统过频"},
            {"SystemUnderFrequency","系统欠频"},
            {"DCChargingOvercurrent","直流充电过流"},
            {"DCDischargingOvercurrent","直流放电过流"},
            {"IslandProtection","孤岛保护"},
            {"ReservedFaultAndStatus","预留故障+状态位"},
            {"VoltagePhaseA","A相电压"},
            {"VoltagePhaseB","B相电压"},
            {"VoltagePhaseC","C相电压"},
            {"CurrentPhaseA","A相电流"},
            {"CurrentPhaseB","B相电流"},
            {"CurrentPhaseC","C相电流"},
            {"GridFrequency","电网频率"},
            {"ActivePowerPhaseA","A相有功功率"},
            {"ActivePowerPhaseB","B相有功功率"},
            {"ActivePowerPhaseC","C相有功功率"},
            {"TotalActivePower","总有功功率"},
            {"ReactivePowerPhaseA","A相无功功率"},
            {"ReactivePowerPhaseB","B相无功功率"},
            {"ReactivePowerPhaseC","C相无功功率"},
            {"TotalReactivePower","总无功功率"},
            {"ApparentPowerPhaseA","A相视在功率"},
            {"ApparentPowerPhaseB","B相视在功率"},
            {"ApparentPowerPhaseC","C相视在功率"},
            {"TotalApparentPower","总视在功率"},
            {"PowerFactorPhaseA","A相功率因数"},
            {"PowerFactorPhaseB","B相功率因数"},
            {"PowerFactorPhaseC","C相功率因数"},
            {"TotalPowerFactor","总功率因数"},
            {"PCSEntryPower","PCS输入功率"},
            {"PCSEntryVoltage","PCS输入电压"},
            {"PCSEntryCurrent","PCS输入电流"},
            {"DailyChargeAmount","日充电量"},
            {"DailyDischargeAmount","日放电量"},
            {"ACCumulativeChargeAmount","交流累计充电量"},
            {"ACCumulativeDischargeAmount","交流累计放电量"},
            {"DCCumulativeChargeAmount","直流累计充电量"},
            {"DCCumulativeDischargeAmount","直流累计放电量"},
            {"MaxAllowableChargeCurrent","最大允许充电电流"},
            {"MaxAllowableDischargeCurrent","最大允许放电电流"},
            {"MaxAllowableChargePower","最大允许充电功率"},
            {"MaxAllowableDischargePower","最大允许放电功率"},
            {"IGBTTemperature","IGBT温度"},
            {"PCSFaultWord1","PCS故障字1"},
            {"PCSFaultWord2","PCS故障字2"},
            {"PCSFaultWord3","PCS故障字3"},
            {"PCSFaultWord4","PCS故障字4"},
            {"PCSFaultWord5","PCS故障字5"},
            {"PCSFaultWord6","PCS故障字6"},
            {"SystemClockSeconds","系统时钟-秒"},
            {"SystemClockMinutes","系统时钟-分"},
            {"SystemClockHours","系统时钟-时"},
            {"SystemClockDay","系统时钟-日"},
            {"SystemClockMonth","系统时钟-月"},
            {"SystemClockYear","系统时钟-年"},
            {"Reserved2","预留2"},
            {"RemoteLocalSetting","远程本地设置"},
            {"OperatingModeSetting","运行模式设置"},
            {"GridConnectionDisconnectionSetting","并网离网设置"},
            {"DevicePowerOn","设备开机"},
            {"DevicePowerOff","设备停机"},
            {"ConstantPowerActivePowerSetting","设备开关机指令"},
            {"ConstantPowerReactivePowerSetting","恒功率有功功率设置"},
            {"PowerFactorControl","恒功率无功功率设置"},
            {"ReservedRemoteAdjustmentAndControl","功率因数控制"}

        };

    }
}
