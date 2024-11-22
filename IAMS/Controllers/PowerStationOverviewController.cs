using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
    public class PowerStationOverviewController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
