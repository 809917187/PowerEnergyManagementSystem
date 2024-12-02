using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
    public class ElectricityReportController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
