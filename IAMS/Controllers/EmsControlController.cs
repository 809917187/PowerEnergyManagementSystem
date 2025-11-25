using IAMS.Models.EmsControl;
using IAMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
	public class EmsControlController : Controller {
		private readonly IPowerStationService _powerStationService;
		private readonly IEmsControlService _emsControlService;
		public EmsControlController(IPowerStationService powerStationService, IEmsControlService emsControlService) {
			_powerStationService = powerStationService;
			_emsControlService = emsControlService;
		}

		[HttpGet]
		public async Task<IActionResult> Index(string energyStorageCabinetSn) {
			if (string.IsNullOrWhiteSpace(energyStorageCabinetSn)) {
				energyStorageCabinetSn = _powerStationService.GetDefaultSelectedEmsSn();
			}
			//从MQTT获取数据
			var viewModel = await _emsControlService.GetEmsControlViewModel(energyStorageCabinetSn);

			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> TestModeControl([FromBody] TestModeModel testMode) {
			//下发到MQTT
			var res = await _emsControlService.SendTestModelMessage(testMode);
			if (res.respCode == 0) {
				return Ok(new { msg = res.respMsg });
			} else { 
				return BadRequest(new { msg = res.respMsg });
			}
		}


		[HttpPost]
		public async Task<IActionResult> ProtectSettingControl([FromBody] ProtectSettingModel testMode) {
			//下发到MQTT
			var res = await _emsControlService.SendProtectSettingMessage(testMode);
			if (res.respCode == 0) {
				return Ok(new { msg = res.respMsg });
			} else {
				return BadRequest(new { msg = res.respMsg });
			}
		}

		[HttpPost]
		public async Task<IActionResult> PowerUageControl([FromBody] PowerUsageModel testMode) {
			//下发到MQTT
			var res = await _emsControlService.SendPowerUsageMessage(testMode);
			if (res.respCode == 0) {
				return Ok(new { msg = res.respMsg });
			} else {
				return BadRequest(new { msg = res.respMsg });
			}
		}
		

		[HttpPost]
		public async Task<IActionResult> PvStorageControl([FromBody] PvStorageModel model) {
			//下发到MQTT
			var res = await _emsControlService.SendPvStorageMessage(model);
			if (res.respCode == 0) {
				return Ok(new { msg = res.respMsg });
			} else {
				return BadRequest(new { msg = res.respMsg });
			}
		}
	}
}
