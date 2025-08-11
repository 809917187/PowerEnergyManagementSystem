namespace IAMS.MQTT.Model {
    public class devData {
        public int devType { get; set; } // 设备类型
        public string devName { get; set; } // 设备名称
        public int devId { get; set; }   // 设备ID
        public string sn { get; set; }      // 序列号
        public int currentPack { get; set; }
        public int totalPack { get; set; }
        public Dictionary<string, float> data { get; set; } // 数据字典
    }

    public class DeviceDataFromMqtt {
        public long timeStamp { get; set; } // 毫秒时间戳
        public string emsSn { get; set; } // 毫秒时间戳
        public List<devData> devData { get; set; } // 设备数据列表
    }
}
