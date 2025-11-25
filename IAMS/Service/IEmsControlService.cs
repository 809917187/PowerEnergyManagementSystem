using IAMS.Models.EmsControl;
using IAMS.ViewModels.EmsControl;

namespace IAMS.Service {
	public interface IEmsControlService {

		public Task<EmsControlViewModel> GetEmsControlViewModel(string energyStorageCabinetSn);
		public Task<TestModeModel> SendTestModelMessage(TestModeModel model);
		public Task<PowerUsageModel> SendPowerUsageMessage(PowerUsageModel model);
		public Task<ProtectSettingModel> SendProtectSettingMessage(ProtectSettingModel model);
		public Task<PvStorageModel> SendPvStorageMessage(PvStorageModel model);

	}
}
