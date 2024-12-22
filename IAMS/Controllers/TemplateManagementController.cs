using IAMS.Models.PriceTemplate;
using IAMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
    public class TemplateManagementController : Controller {
        private readonly ITemplateService _templateService;
        public TemplateManagementController(ITemplateService templateService) {
            _templateService = templateService;
        }
        public IActionResult Index() {
            return View(_templateService.GetAllPriceTemplateInfos());
        }

        [HttpGet]
        public IActionResult AddTemplate() {
            PriceTemplateInfo priceTemplateInfo = new PriceTemplateInfo();
            return View(priceTemplateInfo.TimeFrameTypeCode2Name);
        }

        [HttpPost]
        public IActionResult AddTemplate([FromBody] PriceTemplateInfo priceTemplate) {
            priceTemplate.CreaterId = Convert.ToInt32(User.FindFirst("UserId").Value);
            if (_templateService.AddTemplate(priceTemplate)) {
                return Ok(new { message = "模板新建成功" });
            } else {
                return BadRequest("模板新建失败");
            }
        }

        [HttpPost]
        public JsonResult DeletePriceTemplate(int id) {
            if (_templateService.DeleteTemplateInfo(id)) {
                return Json(new { success = true, message = "删除成功" });
            } else {
                return Json(new { success = true, message = "删除失败" });
            }
        }

        [HttpGet]
        public IActionResult EditPriceTemplate(int id) {
            return View(_templateService.GetPriceTemplateInfoById(id));
        }

        [HttpPost]
        public IActionResult UpdatePriceTemplate([FromBody] PriceTemplateInfo priceTemplate) {
            if (priceTemplate == null) {
                return BadRequest("更新失败");
            }

            if (_templateService.UpdatePriceTemplate(priceTemplate)) {
                return Json(new { success = true, message = "更新成功" });
            } else {
                return Json(new { success = true, message = "更新失败" });
            }
        }

        [HttpGet]
        public IActionResult GetPriceTemplateById(int id) {
            PriceTemplateInfo priceTemplateInfo = _templateService.GetPriceTemplateInfoById(id);
            return Ok(priceTemplateInfo);
        }
    }
}
