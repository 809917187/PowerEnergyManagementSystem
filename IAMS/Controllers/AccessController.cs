using IAMS.Models.User;
using IAMS.Service;
using IAMS.ViewModels.Access;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace IAMS.Controllers {
    public class AccessController : Controller {
        private readonly IUserService _userService;

        public AccessController(IUserService userService) {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login() {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated) {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserInfo modelLogin) {
            UserInfo user = _userService.GetUserInfoByEmailAndPassword(modelLogin.Email, modelLogin.Password);
            if (user != null) {
                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier,modelLogin.Email),
                    new Claim("RoleName",user.RoleName),
                    new Claim("Name",user.Name),
                    new Claim("Role",user.RoleCode.ToString()),
                    new Claim("UserId",user.Id.ToString())
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties() {
                    AllowRefresh = true,
                    IsPersistent = modelLogin.KeepLoggedIn
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Home");
            }

            ViewData["ValidateMessage"] = "user not found";
            return View();
        }

        public async Task<IActionResult> LogOut() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Access");
        }

        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword, string newPasswordDC) {
            // 验证新密码是否一致
            if (newPassword != newPasswordDC) {
                return Json(new { success = false, message = "新密码和确认密码不一致。" });
            }
            if (int.TryParse(User.FindFirst("UserId")?.Value, out int userId) && userId != 0) {
                bool passwordChanged = _userService.UpdatePassword(oldPassword, newPassword, userId);
                if (passwordChanged) {
                    return Json(new { success = true, message = "密码修改成功！" });
                } else {
                    return Json(new { success = false, message = "旧密码不正确或修改失败。" });
                }
            } else {
                return Json(new { success = false, message = "旧密码不正确或修改失败。" });
            }




        }

        [HttpPost]
        public IActionResult AddUser(UserInfo userInfo) {
            if (_userService.GetAllUsers().FindAll(s => s.Email == userInfo.Email).Count > 0) {
                return Json(new { success = false, message = "添加失败！邮箱已存在！" });
            }

            if (_userService.AddUser(userInfo)) {
                return Json(new { success = true, message = "添加成功！" });
            } else {
                return Json(new { success = false, message = "添加失败。" });
            }
        }


        [HttpGet]
        public IActionResult UserManagement() {
            UserManagementViewModels model = new UserManagementViewModels();
            model.userInfos = _userService.GetAllUsers();

            return View(model);
        }
    }
}
