using IAMS.Models.PowerStation;
using IAMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
    public class PowerStationManagementController : Controller {
        private readonly IPowerStationService _powerStationService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUserService _userService;

        public PowerStationManagementController(IPowerStationService powerStationService, IWebHostEnvironment hostingEnvironment, IUserService userService) {
            _powerStationService = powerStationService;
            _hostingEnvironment = hostingEnvironment;
            _userService = userService;
        }

        public IActionResult Index() {
            return View(_powerStationService.GetAllPowerStationInfos());
        }

        [HttpGet]
        public IActionResult AddPowerStation() {
            return View();
        }

        [HttpGet]
        public IActionResult EditPowerStation(int id) {
            return View(_powerStationService.GetPowerStationInfoById(id));
        }
        [HttpGet]
        public IActionResult DeletePowerStationImage(string key) {
            return Ok(new { message = "电站信息更新成功" });
        }

        [HttpPost]
        public async Task<IActionResult> EditPowerStation(PowerStationInfo powerStationInfo, List<string> DeletedImages) {
            if (await this.UploadImages(powerStationInfo) && _powerStationService.UpdateStationInfo(powerStationInfo)) {
                return Ok(new { message = "电站信息更新成功" });
            } else {
                return BadRequest("电站信息更新失败");
            }
        }

        [HttpPost]
        public async Task<IActionResult> BindCabinetToPowerStation([FromBody] BindRequestModel bindRequestModel) {
            if (_powerStationService.BindCabinetToPowerStation(bindRequestModel.powerStationId, bindRequestModel.cabinetIds)) {
                return Ok(new { message = "绑定成功" });
            } else {
                return BadRequest("绑定失败");
            }
        }


        public async Task<IActionResult> BindPowerStationToUser([FromBody] BindRequestModel bindRequestModel) {
            if (_powerStationService.BindPowerStationToUser(bindRequestModel.powerStationId, bindRequestModel.userIds)) {
                return Ok(new { message = "绑定成功" });
            } else {
                return BadRequest("绑定失败");
            }
        }

        [HttpGet]
        public IActionResult GetPowerStationInfoById(int id) {
            var ret = _powerStationService.GetPowerStationInfoById(id);

            return Ok(ret);
        }
        [HttpGet]
        public IActionResult GetAllCabinetList() {
            var ret = _powerStationService.GetAllEnergyStorageCabinetArray();

            return Ok(ret);
        }


        public async Task<IActionResult> AddPowerStation([FromForm] PowerStationInfo model) {
            try {
                //电站名称存在，返回
                var powerStationInfos = _powerStationService.GetAllPowerStationInfos();
                if (powerStationInfos.FindAll(s => s.Name == model.Name).Count > 0) {
                    return Ok(new { status = 500, message = "电站名称已经存在了" });
                }

                //上传安装图片
                if (await this.UploadImages(model) && _powerStationService.AddPowerSatationInfo(model)) {
                    return Ok(new { status = 200, message = "电站新建成功" });
                } else {
                    return BadRequest("电站新建失败");
                }
            } catch (Exception ex) {
                return BadRequest("电站新建失败");
            }

        }

        private async Task<bool> UploadImages(PowerStationInfo model) {
            try {
                //上传电站图片
                // 上传电站图片
                var uploadsFolder = Path.Combine("images", "stationImages");  // 相对路径部分
                var absoluteUploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, uploadsFolder);

                // 如果文件夹不存在，则创建它
                if (!Directory.Exists(absoluteUploadsFolder)) {
                    Directory.CreateDirectory(absoluteUploadsFolder);
                }
                foreach (var file in model.StationImages) {
                    if (file.Length > 0) {
                        // 生成UUID作为文件名
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(absoluteUploadsFolder, fileName);

                        // 保存文件
                        using (var stream = new FileStream(filePath, FileMode.Create)) {
                            await file.CopyToAsync(stream);
                            var relativePath = Path.Combine(uploadsFolder, fileName).Replace("\\", "/");
                            model.StationImagesFilePath.Add(relativePath);
                        }
                    }
                }
                foreach (var file in model.StationInstallImages) {
                    if (file.Length > 0) {
                        // 生成UUID作为文件名
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(absoluteUploadsFolder, fileName);

                        // 保存文件
                        using (var stream = new FileStream(filePath, FileMode.Create)) {
                            await file.CopyToAsync(stream);
                            var relativePath = Path.Combine(uploadsFolder, fileName).Replace("\\", "/");
                            model.StationInstallImagesFilePath.Add(relativePath);
                        }
                    }
                }
            } catch (Exception ex) {
                return false;
            }
            return true;

        }
        public async Task<IActionResult> UploadStationImage([FromForm] List<IFormFile> files) {
            IFormFile file = files[0];
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "station_images");

            Directory.CreateDirectory(uploadDirectory);
            var filePath = Path.Combine(uploadDirectory, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create)) {
                await file.CopyToAsync(stream);
            }

            return Ok(new { filePath, message = "File uploaded successfully!" });
        }

        [HttpPost]
        public JsonResult DeletePowerStation(int id) {
            if (_powerStationService.DeletePowerSatationInfo(id)) {
                return Json(new { success = true, message = "删除成功" });
            } else {
                return Json(new { success = true, message = "删除失败" });
            }
        }

        [HttpGet]
        public IActionResult GetAllBindUserList(int powerStationId) {
            var ret = _userService.GetAllUsers();

            var bindedUser = _powerStationService.GetBindUserListByPowerStationId(powerStationId);

            foreach (var user in ret) {
                if (bindedUser.Contains(user.Id)) {
                    user.IsChecked = true;
                }
            }

            return Ok(ret);
        }
    }
}
