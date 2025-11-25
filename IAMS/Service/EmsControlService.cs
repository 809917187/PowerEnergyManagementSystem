using IAMS.Models.EmsControl;
using IAMS.MQTT;
using IAMS.ViewModels.EmsControl;

namespace IAMS.Service {
	public class EmsControlService : IEmsControlService {
		private string _connectionString;
		private IPowerStationService _powerStationService;

		public EmsControlService(IConfiguration configuration, IPowerStationService powerStationService,
					IStationSystemService stationSystemService, IClickHouseService clickHouseService) {
			_connectionString = configuration.GetConnectionString("ems");
			_powerStationService = powerStationService;
		}

		public async Task<EmsControlViewModel> GetEmsControlViewModel(string energyStorageCabinetSn) {
			EmsControlViewModel model = new EmsControlViewModel();
			model.PowerStationInfos = _powerStationService.GetAllPowerStationInfos(0, energyStorageCabinetSn);

			//从mqtt获取TetsMode的数据
			MQTTHelper mqttHelper = new MQTTHelper();
			model.testModeModel = await mqttHelper.GetTestModeModel(energyStorageCabinetSn);
			model.protectSettingModel = await mqttHelper.GetProtectSettingModel(energyStorageCabinetSn);
			model.powerUsageModel = await mqttHelper.GetPowerUsageModel(energyStorageCabinetSn);
			model.pvStorageModel =await mqttHelper.GetPvStorageModel(energyStorageCabinetSn);
			return model;
		}

		public async Task<PowerUsageModel> SendPowerUsageMessage(PowerUsageModel model) {
			MQTTHelper mqttHelper = new MQTTHelper();
			var res = await mqttHelper.SendPowerUsageModel(model);
			return res;
		}

		public async Task<ProtectSettingModel> SendProtectSettingMessage(ProtectSettingModel model) {
			MQTTHelper mqttHelper = new MQTTHelper();
			var res = await mqttHelper.SendProtectSettingModel(model);
			return res;
		}

		public async Task<TestModeModel> SendTestModelMessage(TestModeModel testModeModel) {
			MQTTHelper mqttHelper = new MQTTHelper();
			var res = await mqttHelper.SendTestModeModel(testModeModel);
			return res;
		}

		public async Task<PvStorageModel> SendPvStorageMessage(PvStorageModel model) {
			MQTTHelper mqttHelper = new MQTTHelper();
			var res = await mqttHelper.SendPvStorageModel(model);
			return res;
		}

	}
}
