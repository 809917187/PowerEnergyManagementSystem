using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
    public class PowerStationManagementController : Controller {
        public IActionResult Index() {
            return View();
        }


        public IActionResult AddPowerStation() {
            return View();
        }
    }
}
