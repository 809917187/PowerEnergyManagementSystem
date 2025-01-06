namespace IAMS.MQTT.Model {
    public class DataFromMqtt {
        public string devType { get; set; } // 设备类型
        public string devName { get; set; } // 设备名称
        public string devId { get; set; }   // 设备ID
        public string sn { get; set; }      // 序列号
        public Dictionary<string, double> data { get; set; } // 数据字典
    }

    public class DeviceDataFromMqtt {
        public long timeStamp { get; set; } // 毫秒时间戳
        public List<DataFromMqtt> devData { get; set; } // 设备数据列表
    }
}
