namespace IAMS.MQTT.Model {
    public class RootDataFromMqtt {
        public Structure structure { get; set; }
    }

    public class Structure {
        public int menuTree { get; set; } // 节点类型
        public string name { get; set; } // 名称
        public int devType { get; set; } // 设备类型
        public string sn { get; set; } // SN编号
        public List<Structure> child { get; set; } // 子节点
    }
}
