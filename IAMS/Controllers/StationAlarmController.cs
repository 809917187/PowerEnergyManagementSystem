using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
    public class StationAlarmController : Controller {
        public IActionResult Index() {
            return View();
        }

        public IActionResult AlarmHistory() {
            return View();
        }
    }

    
}
