using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
    public class BigScreenController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
