using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
    public class MultiStationOverviewController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
