using IAMS.Models.PowerStation;
using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
    public class PowerStationManagementController : Controller {
        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public IActionResult AddPowerStation() {
            return View();
        }

        public IActionResult AddPowerStation([FromForm] PowerStationInfo model) {

            return Ok(new { message = "文件上传成功" });
        }
    }
}
