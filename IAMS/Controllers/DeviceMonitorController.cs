using IAMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
    public class DeviceMonitorController : Controller {
        private readonly IDeviceMonitorService _deviceMonitorService;
        public DeviceMonitorController(IDeviceMonitorService deviceMonitorService) {
            _deviceMonitorService = deviceMonitorService;
        }
        public IActionResult Index(string energyStorageCabinetName) {
            return View(_deviceMonitorService.GetDeviceMonitorViewModel(energyStorageCabinetName, DateTime.Now));
        }
    }
}
