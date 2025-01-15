namespace IAMS.Models.StationSystem {
    public class GatewayTableModelInfo : DeviceDataBaseInfo {
        public bool IsEnabled { get; set; }     //	是否启用	0
        public bool IsOnline { get; set; }      //	是否在线	1
        public double VoltagePhaseA { get; set; } = 0;  //	A相电压	2
        public double VoltagePhaseB { get; set; } = 0;  //	B相电压	3
        public double VoltagePhaseC { get; set; } = 0;  //	C相电压	4
        public double VoltageLineAB { get; set; } = 0;  //	AB线电压	5
        public double VoltageLineBC { get; set; } = 0;  //	BC线电压	6
        public double VoltageLineCA { get; set; } = 0;  //	CA线电压	7
        public double CurrentPhaseA { get; set; } = 0;  //	A相电流	8
        public double CurrentPhaseB { get; set; } = 0;  //	B相电流	9
        public double CurrentPhaseC { get; set; } = 0;  //	C相电流	10
        public double ActivePowerPhaseA { get; set; } = 0;  //	A相有功功率	11
        public double ActivePowerPhaseB { get; set; } = 0;  //	B相有功功率	12
        public double ActivePowerPhaseC { get; set; } = 0;  //	C相有功功率	13
        public double TotalActivePower { get; set; } = 0;   //	总有功功率	14
        public double ReactivePowerPhaseA { get; set; } = 0;    //	A相无功功率	15
        public double ReactivePowerPhaseB { get; set; } = 0;    //	B相无功功率	16
        public double ReactivePowerPhaseC { get; set; } = 0;    //	C相无功功率	17
        public double TotalReactivePower { get; set; } = 0; //	总无功功率	18
        public double ApparentPowerPhaseA { get; set; } = 0;    //	A相视在功率	19
        public double ApparentPowerPhaseB { get; set; } = 0;    //	B相视在功率	20
        public double ApparentPowerPhaseC { get; set; } = 0;    //	C相视在功率	21
        public double TotalApparentPower { get; set; } = 0; //	总视在功率	22
        public double PowerFactorPhaseA { get; set; } = 0;  //	A相功率因数	23
        public double PowerFactorPhaseB { get; set; } = 0;  //	B相功率因数	24
        public double PowerFactorPhaseC { get; set; } = 0;  //	C相功率因数	25
        public double TotalPowerFactor { get; set; } = 0;   //	总功率因数	26
        public double GridFrequency { get; set; } = 0;  //	电网频率	27
        public double VoltageTransformationRatio { get; set; } = 0; //	电压互感比	28
        public double CurrentTransformationRatio { get; set; } = 0; //	电流互感比	29
        public double ForwardActiveEnergy { get; set; } = 0;    //	正向有功电度	30
        public double ReverseActiveEnergy { get; set; } = 0;    //	反向有功电度	31
        public double ForwardReactiveEnergy { get; set; } = 0;  //	正向无功电度	32
        public double ReverseReactiveEnergy { get; set; } = 0;  //	反向无功电度	33
        public double PeakForwardActiveEnergy { get; set; } = 0;    //	尖正向有功电度	34
        public double PeakReverseActiveEnergy { get; set; } = 0;    //	尖反向有功电度	35
        public double FlatForwardActiveEnergy { get; set; } = 0;    //	峰正向有功电度	36
        public double FlatReverseActiveEnergy { get; set; } = 0;    //	峰反向有功电度	37
        public double NormalForwardActiveEnergy { get; set; } = 0;  //	平正向有功电度	38
        public double NormalReverseActiveEnergy { get; set; } = 0;  //	平反向有功电度	39
        public double ValleyForwardActiveEnergy { get; set; } = 0;  //	谷正向有功电度	40
        public double ValleyReverseActiveEnergy { get; set; } = 0;  //	谷反向有功电度	41
        public double CurrentMonthMaxForwardActiveDemand { get; set; } = 0; //	当月正向有功最大需量	42
        public double CurrentMonthMaxReverseActiveDemand { get; set; } = 0;	//	当月反向有功最大需量	43
        public Dictionary<string, string> Property2ChineseName = new Dictionary<string, string>() {
            {"IsEnabled","是否启用"},
            {"IsOnline","是否在线"},
            {"VoltagePhaseA","A相电压"},
            {"VoltagePhaseB","B相电压"},
            {"VoltagePhaseC","C相电压"},
            {"VoltageLineAB","AB线电压"},
            {"VoltageLineBC","BC线电压"},
            {"VoltageLineCA","CA线电压"},
            {"CurrentPhaseA","A相电流"},
            {"CurrentPhaseB","B相电流"},
            {"CurrentPhaseC","C相电流"},
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
            {"GridFrequency","电网频率"},
            {"VoltageTransformationRatio","电压互感比"},
            {"CurrentTransformationRatio","电流互感比"},
            {"ForwardActiveEnergy","正向有功电度"},
            {"ReverseActiveEnergy","反向有功电度"},
            {"ForwardReactiveEnergy","正向无功电度"},
            {"ReverseReactiveEnergy","反向无功电度"},
            {"PeakForwardActiveEnergy","尖正向有功电度"},
            {"PeakReverseActiveEnergy","尖反向有功电度"},
            {"FlatForwardActiveEnergy","峰正向有功电度"},
            {"FlatReverseActiveEnergy","峰反向有功电度"},
            {"NormalForwardActiveEnergy","平正向有功电度"},
            {"NormalReverseActiveEnergy","平反向有功电度"},
            {"ValleyForwardActiveEnergy","谷正向有功电度"},
            {"ValleyReverseActiveEnergy","谷反向有功电度"},
            {"CurrentMonthMaxForwardActiveDemand","当月正向有功最大需量"},
            {"CurrentMonthMaxReverseActiveDemand","当月反向有功最大需量"}
        };
    }
}
