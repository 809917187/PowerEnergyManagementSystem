using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
    public class EnergyManagementController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
