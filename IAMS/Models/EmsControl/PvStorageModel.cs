using System.Text.Json.Serialization;

namespace IAMS.Models.EmsControl {
	public class PvStorageModel {
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
		public int submode { get; set; }
		public PvStorageLogicCfg logicCfg { get; set; }
	}
	public class PvStorageLogicCfg {
		public int pvInverterBrand { get; set; }
		public int pcsBrand { get; set; }
		public int pvCouplingMethod { get; set; }
		public int OnThreshold { get; set; }
		public int offThreshold { get; set; }
		public int pvMaxPower { get; set; }
		public int maxGridPower { get; set; }
		public int transRedunPower { get; set; }
	}

}
