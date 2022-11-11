using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NDWebApp.Data;
using NDWebApp.Models;
using NDWebApp.Areas.Identity.Data;

namespace NDWebApp.Controllers
{
    public class UserAuthController : Controller
    {
        private readonly UserManager<NDWebAppUser> _userManager;
        private readonly SignInManager<NDWebAppUser> _signInManager;
        private readonly NDWebAppContext _context;

        public UserAuthController(NDWebAppContext context,
            UserManager<NDWebAppUser> userManager,
            SignInManager<NDWebAppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            loginModel.LoginInvalid = "true";

            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.Email,
                                                                        loginModel.Password,
                                                                        loginModel.RememberMe,
                                                                        lockoutOnFailure: false);
                if(result.Succeeded)
                {
                    loginModel.LoginInvalid = "";
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Invalid login attempt");
                }

            }
            return PartialView("_UserLoginPartial", loginModel);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            if(returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
