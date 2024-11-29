using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
    public class DeviceMonitorController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
