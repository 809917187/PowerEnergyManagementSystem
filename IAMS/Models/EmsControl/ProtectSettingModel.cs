using System.Text.Json.Serialization;

namespace IAMS.Models.EmsControl {
	public class ProtectSettingModel {
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string sn { get; set; }
		//public string transaction { get; set; }
		public int transaction { get; set; }
		public int timeStamp { get; set; }
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public int respCode { get; set; }
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string respMsg { get; set; }
        public int runMode { get; set; } = 100;
		public ProtectSettingCfg logicCfg { get; set; }
	}

	public class ProtectSettingCfg {
		public int transCapacity { get; set; }
		public int maxPower { get; set; }
		public int overLoadSwitch { get; set; }
		public int olWarnLimitVal { get; set; }
		public int olShutdownVal { get; set; }
		public int demandSwitch { get; set; }
		public int targetDemand { get; set; }
		public int deWarnLimitVal { get; set; }
		public int deShutdownVal { get; set; }
		public int backflowSwitch { get; set; }
		public int bfWarnLimitVal { get; set; }
		public int bfShutdownVal { get; set; }
		public int socForbidCharge { get; set; }
		public int socForbidDischarge { get; set; }

	}
}
