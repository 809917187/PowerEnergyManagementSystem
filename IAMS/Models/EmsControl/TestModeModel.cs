using System.Text.Json.Serialization;

namespace IAMS.Models.EmsControl {
	public class TestModeModel {
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string sn { get; set; }
		public int transaction { get; set; }
		public int timeStamp { get; set; }
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public int respCode { get; set; }
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string respMsg { get; set; }
		public int runMode { get; set; }=0;
		public TestModeLogicCfg logicCfg { get; set; }
	}


	public class TestModeLogicCfg {
		public int activePower { get; set; }

		public int onOff { get; set; } 
		public int reactivePower { get; set; }
		public int protectSwitch { get; set; }
	}

}
