using IAMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
    public class DeviceMonitorController : Controller {
        private readonly IDeviceMonitorService _deviceMonitorService;
        private readonly IPowerStationService _powerStationService;
        public DeviceMonitorController(IDeviceMonitorService deviceMonitorService, IPowerStationService powerStationService) {
            _deviceMonitorService = deviceMonitorService;
            _powerStationService = powerStationService;
        }
        public IActionResult Index(string energyStorageCabinetSn) {
            if (string.IsNullOrWhiteSpace(energyStorageCabinetSn)) {
                energyStorageCabinetSn = _powerStationService.GetDefaultSelectedEmsSn();
            }
            return View(_deviceMonitorService.GetDeviceMonitorViewModel(energyStorageCabinetSn));
        }
    }
}
