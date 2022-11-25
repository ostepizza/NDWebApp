using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using NDWebApp.Areas.Identity.Data;
using NDWebApp.Data;
using NDWebApp.Models;
using System.Diagnostics;

namespace NDWebApp.MVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<NDWebAppUser> userManager;
        private IPasswordHasher<NDWebAppUser> passwordHasher;
        private ILogger<UsersController> _logger;
        private readonly IUsersSqlConnector usersSqlConnector;

        public AccountController(UserManager<NDWebAppUser> usrMgr, IPasswordHasher<NDWebAppUser> passwordHash, ILogger<UsersController> logger, IUsersSqlConnector usersSqlConnector)
        {
            userManager = usrMgr;
            passwordHasher = passwordHash;
            _logger = logger;
            this.usersSqlConnector = usersSqlConnector;
        }
        public IActionResult Index()
        {
            UserModel model = usersSqlConnector.GetUserById(userManager.GetUserAsync(User).Result.Id);
            model.SuggestionsSubmitted = usersSqlConnector.GetUserSuggestions(userManager.GetUserAsync(User).Result.Id);
            model.RepairsSubmitted = usersSqlConnector.GetUserRepairs(userManager.GetUserAsync(User).Result.Id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string password, string confirmPassword)
        {
            NDWebAppUser user = await userManager.FindByIdAsync(userManager.GetUserAsync(User).Result.Id);
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    if (!string.IsNullOrEmpty(password) && (password == confirmPassword))
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, password);
                        IdentityResult result = await userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            TempData["Message"] = "Passordet ditt er blitt endret.";
                            TempData["Status"] = "Success";
                            return RedirectToAction("Index");
                        }
                        else
                            Errors(result);
                    }
                    else
                    TempData["Message"] = "Passord-feltene er ikke like.";
                    TempData["Status"] = "Danger";
                    ModelState.AddModelError("", "Password cannot be empty");
                    return RedirectToAction("Index");
                }
                else
                    TempData["Message"] = "Det skjedde noe galt.";
                    TempData["Status"] = "Warning";
                    ModelState.AddModelError("", "User Not Found");
                    return RedirectToAction("Index");

            }
            TempData["Message"] = "Passord-feltene kan ikke være tomme.";
            TempData["Status"] = "Danger";
            return RedirectToAction("Index");
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}