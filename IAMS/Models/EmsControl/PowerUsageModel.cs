namespace IAMS.Models.EmsControl {
	public class PowerUsageModel {
		public string sn { get; set; }
		public int transaction { get; set; }
		public int timeStamp { get; set; }
		public int respCode { get; set; }
		public string respMsg { get; set; }
		public int runMode { get; set; } = 1;
		public int subMode { get; set; }
		public double pcsChgPlanPower { get; set; }
		public double transRedunPower { get; set; }
		public PowerUsageLogicCfg logicCfg { get; set; }
	}
	public class PowerUsageLogicCfg {
		public PowerUsagePvTab pvTab { get; set; }
	}
	public class PowerUsagePvTab {
		/*public int dTabN { get; set; }*/
		public PowerUsageTemplate[] template { get; set; }
	}
	public class PowerUsageTemplate {
		public int tltId { get; set; }
		public string tltName { get; set; }
		public int sTabN { get; set; }
		public PowerUsageSTab[] sTab { get; set; }
		public PowerUsageApplyDates[] applyDates { get; set; }
	}
	public class PowerUsageSTab {
		public int sSec { get; set; }
		public int eSec { get; set; }
		public float pwrKw { get; set; }
	}
	public class PowerUsageApplyDates {
		public string sDate { get; set; }
		public string eDate { get; set; }
	}
}
