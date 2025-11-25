using IAMS.Models.EmsControl;
using IAMS.Models.PowerStation;

namespace IAMS.ViewModels.EmsControl {
	public class EmsControlViewModel {
		public List<PowerStationInfo> PowerStationInfos { get; set; } = new List<PowerStationInfo>();
		public TestModeModel testModeModel { get; set; }
		public PowerUsageModel powerUsageModel { get; set; }
		public ProtectSettingModel protectSettingModel { get; set; }
		public PvStorageModel pvStorageModel { get; set; }
    }
}
