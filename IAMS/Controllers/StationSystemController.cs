using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
    public class StationSystemController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
