using IAMS.AttributeTag;
using System.ComponentModel.DataAnnotations;

namespace IAMS.Models.DeviceInfo {
    public class EmsModel000: DeviceBaseInfo {
        [Display(Name = "本地硬件故障"), PointIndex(0)]
        public int LocalHardwareFault { get; set; }

        [Display(Name = "CPU总利用率"), PointIndex(1)]
        public int CpuTotalUsage { get; set; }

        [Display(Name = "当前进程CPU占用"), PointIndex(2)]
        public int CurrentProcessCpuUsage { get; set; }

        [Display(Name = "内存总大小"), PointIndex(3)]
        public int TotalMemorySize { get; set; }

        [Display(Name = "内存占用率"), PointIndex(4)]
        public int MemoryUsageRate { get; set; }

        [Display(Name = "当前进程内存占用"), PointIndex(5)]
        public int CurrentProcessMemoryUsage { get; set; }

        [Display(Name = "网络状态"), PointIndex(6)]
        public int NetworkStatus { get; set; }

        [Display(Name = "EMS运行心跳"), PointIndex(7)]
        public int EMSHeartbeat { get; set; }

        [Display(Name = "DI1信号~DI40信号"), PointRange(8, 47)]
        public int[] DISignals { get; set; }

        [Display(Name = "DO1输出~DO40输出"), PointRange(48, 87)]
        public int[] DOOutputs { get; set; }

        [Display(AutoGenerateField = false), PointRange(88, 97)]
        public int[] Reserved1 { get; set; }

        [Display(Name = "EMS数量"), PointIndex(98)]
        public int EMSCount { get; set; }

        [Display(Name = "关口电表数量"), PointIndex(99)]
        public int GatewayMeterCount { get; set; }

        [Display(Name = "储能电表数量"), PointIndex(100)]
        public int EnergyStorageMeterCount { get; set; }

        [Display(Name = "储能堆控数量"), PointIndex(101)]
        public int EnergyStorageStackControllerCount { get; set; }

        [Display(Name = "储能簇控数量"), PointIndex(102)]
        public int EnergyStorageClusterControllerCount { get; set; }

        [Display(Name = "储能PCS数量"), PointIndex(103)]
        public int EnergyStoragePCSCount { get; set; }

        [Display(Name = "空调液冷机组数量"), PointIndex(104)]
        public int AirConditioningLiquidCoolingUnitCount { get; set; }

        [Display(Name = "温湿度传感器数量"), PointIndex(105)]
        public int TemperatureHumiditySensorCount { get; set; }

        [Display(Name = "水浸数量"), PointIndex(106)]
        public int WaterLeakageCount { get; set; }

        [Display(Name = "烟感数量"), PointIndex(107)]
        public int SmokeDetectorCount { get; set; }

        [Display(Name = "消防系统数量"), PointIndex(108)]
        public int FireProtectionSystemCount { get; set; }

        [Display(Name = "IO模块数量"), PointIndex(109)]
        public int IOModuleCount { get; set; }

        [Display(Name = "预留设备1数量"), PointIndex(110)]
        public int ReservedDevice1Count { get; set; }

        [Display(Name = "预留设备2数量"), PointIndex(111)]
        public int ReservedDevice2Count { get; set; }

        [Display(Name = "预留设备3数量"), PointIndex(112)]
        public int ReservedDevice3Count { get; set; }

        [Display(Name = "预留设备4数量"), PointIndex(113)]
        public int ReservedDevice4Count { get; set; }

        [Display(Name = "预留设备5数量"), PointIndex(114)]
        public int ReservedDevice5Count { get; set; }

        [Display(AutoGenerateField = false), PointRange(115, 199)]
        public int[] Reserved2 { get; set; }

    }
}
