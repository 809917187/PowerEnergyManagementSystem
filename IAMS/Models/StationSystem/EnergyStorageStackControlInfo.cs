﻿using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace IAMS.Models.StationSystem {
    public class EnergyStorageStackControlInfo : DeviceDataBaseInfo {
        public bool IsEnabled { get; set; } //	设备启用	0
        public bool IsOnline { get; set; }  //	设备在线	1
        public double TotalVoltage { get; set; } = 0;   //	总压	2
        public double TotalCurrent { get; set; } = 0;   //	总电流	3
        public double StateOfCharge { get; set; } = 0;  //	SOC	4
        public double StateOfHealth { get; set; } = 0;  //	SOH	5
        public double StateOfEnergy { get; set; } = 0;  //	SOE	6
        public double RatedTotalVoltage { get; set; } = 0;  //	额定总压	7
        public double RatedCapacity { get; set; } = 0;  //	额定容量	8
        public double RemainingCapacity { get; set; } = 0;  //	剩余容量	9
        public double RatedEnergy { get; set; } = 0;    //	额定电量	10
        public double RemainingEnergy { get; set; } = 0;    //	剩余电量	11
        public double TotalNumberOfSlaveUnitsBMU { get; set; } = 0; //	从机总数(BMU)	12
        public double OnlineNumberOfSlaveUnitsBMU { get; set; } = 0;    //	在线从机总数(BMU)	13
        public double TotalNumberOfBatteries { get; set; } = 0; //	电池总数	14
        public double OnlineNumberOfBatteries { get; set; } = 0;    //	在线电池总数	15
        public double TotalNumberOfTemperatureSensors { get; set; } = 0;    //	温感总数	16
        public double OnlineNumberOfTemperatureSensors { get; set; } = 0;   //	在线温感总数	17
        public double MaximumAllowedDischargeCurrent { get; set; } = 0; //	最大允许放电电流	18
        public double MaximumAllowedDischargePower { get; set; } = 0;   //	最大允许放电功率	19
        public double MaximumAllowedChargeCurrent { get; set; } = 0;    //	最大允许充电电流	20
        public double MaximumAllowedChargePower { get; set; } = 0;  //	最大允许充电功率	21
        public double PositiveInsulationResistance { get; set; } = 0;   //	正极绝缘阻值	22
        public double NegativeInsulationResistance { get; set; } = 0;   //	负极绝缘阻值	23
        public double AverageCellVoltage { get; set; } = 0; //	单体平均电压	24
        public double MaximumVoltageDifferenceBetweenCells { get; set; } = 0;   //	单体最大压差	25
        public double HighestCellVoltage { get; set; } = 0; //	最高单体电压	26
        public double HighestCellVoltageSlaveUnitNumber { get; set; } = 0;  //	最高单体电压从机号	27
        public double HighestCellVoltageSerialNumber { get; set; } = 0; //	最高单体电压编号	28
        public double LowestCellVoltage { get; set; } = 0;  //	最低单体电压	29
        public double LowestCellVoltageSlaveUnitNumber { get; set; } = 0;   //	最低单体电压从机号	30
        public double LowestCellVoltageSerialNumber { get; set; } = 0;  //	最低单体电压编号	31
        public double AverageCellTemperature { get; set; } = 0; //	单体平均温度	32
        public double MaximumTemperatureDifference { get; set; } = 0;   //	最大温差	33
        public double HighestCellTemperature { get; set; } = 0; //	最高单体温度	34
        public double HighestCellTemperatureSlaveUnitNumber { get; set; } = 0;  //	最高单体温度从机号	35
        public double HighestCellTemperatureSerialNumber { get; set; } = 0; //	最高单体温度编号	36
        public double LowestCellTemperature { get; set; } = 0;  //	最低单体温度	37
        public double LowestCellTemperatureSlaveUnitNumber { get; set; } = 0;   //	最低单体温度从机号	38
        public double LowestCellTemperatureSerialNumber { get; set; } = 0;  //	最低单体温度编号	39
        public double DailyChargeCapacity { get; set; } = 0;    //	日充电容量	40
        public double DailyChargeEnergy { get; set; } = 0;  //	日充电电量	41
        public double DailyDischargeCapacity { get; set; } = 0; //	日放电容量	42
        public double DailyDischargeEnergy { get; set; } = 0;   //	日放电电量	43
        public double DailyChargeTime { get; set; } = 0;    //	日充电时间	44
        public double DailyDischargeTime { get; set; } = 0; //	日放电时间	45
        public double CumulativeChargeCapacity { get; set; } = 0;   //	累计充电容量	46
        public double CumulativeChargeEnergy { get; set; } = 0; //	累计充电电量	47
        public double CumulativeDischargeCapacity { get; set; } = 0;    //	累计放电容量	48
        public double CumulativeDischargeEnergy { get; set; } = 0;	//	累计放电电量	49
        public Dictionary<string, string> Property2ChineseName = new Dictionary<string, string>() {
            {"IsEnabled","设备启用"},
            {"IsOnline","设备在线"},
            {"TotalVoltage","总压"},
            {"TotalCurrent","总电流"},
            {"StateOfCharge","SOC"},
            {"StateOfHealth","SOH"},
            {"StateOfEnergy","SOE"},
            {"RatedTotalVoltage","额定总压"},
            {"RatedCapacity","额定容量"},
            {"RemainingCapacity","剩余容量"},
            {"RatedEnergy","额定电量"},
            {"RemainingEnergy","剩余电量"},
            {"TotalNumberOfSlaveUnitsBMU","从机总数(BMU)"},
            {"OnlineNumberOfSlaveUnitsBMU","在线从机总数(BMU)"},
            {"TotalNumberOfBatteries","电池总数"},
            {"OnlineNumberOfBatteries","在线电池总数"},
            {"TotalNumberOfTemperatureSensors","温感总数"},
            {"OnlineNumberOfTemperatureSensors","在线温感总数"},
            {"MaximumAllowedDischargeCurrent","最大允许放电电流"},
            {"MaximumAllowedDischargePower","最大允许放电功率"},
            {"MaximumAllowedChargeCurrent","最大允许充电电流"},
            {"MaximumAllowedChargePower","最大允许充电功率"},
            {"PositiveInsulationResistance","正极绝缘阻值"},
            {"NegativeInsulationResistance","负极绝缘阻值"},
            {"AverageCellVoltage","单体平均电压"},
            {"MaximumVoltageDifferenceBetweenCells","单体最大压差"},
            {"HighestCellVoltage","最高单体电压"},
            {"HighestCellVoltageSlaveUnitNumber","最高单体电压从机号"},
            {"HighestCellVoltageSerialNumber","最高单体电压编号"},
            {"LowestCellVoltage","最低单体电压"},
            {"LowestCellVoltageSlaveUnitNumber","最低单体电压从机号"},
            {"LowestCellVoltageSerialNumber","最低单体电压编号"},
            {"AverageCellTemperature","单体平均温度"},
            {"MaximumTemperatureDifference","最大温差"},
            {"HighestCellTemperature","最高单体温度"},
            {"HighestCellTemperatureSlaveUnitNumber","最高单体温度从机号"},
            {"HighestCellTemperatureSerialNumber","最高单体温度编号"},
            {"LowestCellTemperature","最低单体温度"},
            {"LowestCellTemperatureSlaveUnitNumber","最低单体温度从机号"},
            {"LowestCellTemperatureSerialNumber","最低单体温度编号"},
            {"DailyChargeCapacity","日充电容量"},
            {"DailyChargeEnergy","日充电电量"},
            {"DailyDischargeCapacity","日放电容量"},
            {"DailyDischargeEnergy","日放电电量"},
            {"DailyChargeTime","日充电时间"},
            {"DailyDischargeTime","日放电时间"},
            {"CumulativeChargeCapacity","累计充电容量"},
            {"CumulativeChargeEnergy","累计充电电量"},
            {"CumulativeDischargeCapacity","累计放电容量"},
            {"CumulativeDischargeEnergy","累计放电电量"}

        };

    }

}
