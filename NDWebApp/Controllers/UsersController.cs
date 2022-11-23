using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NDWebApp.Areas.Identity.Data;
using NDWebApp.Data;
using NDWebApp.Models;
using System.Diagnostics;

namespace NDWebApp.MVC.Controllers
{
    public class UsersController : Controller
    {
        private UserManager<NDWebAppUser> userManager;
        private IPasswordHasher<NDWebAppUser> passwordHasher;
        private ILogger<UsersController> _logger;
        private readonly IUsersSqlConnector usersSqlConnector;

        public UsersController(UserManager<NDWebAppUser> usrMgr, IPasswordHasher<NDWebAppUser> passwordHash, ILogger<UsersController> logger, IUsersSqlConnector usersSqlConnector)
        {
            userManager = usrMgr;
            passwordHasher = passwordHash;
            _logger = logger;
            this.usersSqlConnector = usersSqlConnector;
        }

        [Authorize(Roles = "Administrator,Team Leader")]
        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        [Authorize(Roles = "Administrator")]
        public ViewResult Add() => View();

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(UserModel user)
        {
            if (ModelState.IsValid)
            {
                NDWebAppUser appUser = new NDWebAppUser
                {
                    empNr = user.empNr,
                    empFname = user.empFname,
                    empLname = user.empLname,
                    UserName = user.Email,
                    Email = user.Email,
                    PhoneNumber = user.Phone
                };

                IdentityResult result = await userManager.CreateAsync(appUser, user.Password);

                if (result.Succeeded) { 
                    TempData["Message"] = "Bruker " + user.Email + " (" + user.empFname + " " + user.empLname + ") ble opprettet.";
                    TempData["Status"] = "Success";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        public IActionResult NotFound()
        {
            return View();
        }

        public IActionResult Search()
        {
            var data = usersSqlConnector.GetMatchingUsers("AINTNOBODYGONNASEARCHTHISTERMAHAAXAXAXHAHAHAHHAAHAHTOMMORELLOGUITAR");
            var model = new UserModel();
            model.Users = data;
            return View(model);
        }

        [HttpPost]
        public IActionResult Search(string search)
        {
            var data = usersSqlConnector.GetMatchingUsers(search);
            var model = new UserModel();
            model.Users = data;
            return View(model);
        }

        [Authorize(Roles = "Administrator,Team Leader")]
        public async Task<IActionResult> View(string id)
        {
            NDWebAppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("NotFound");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(string id)
        {
            NDWebAppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("NotFound");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, string email, string phonenumber, string empfname, string emplname, int empnr)
        {
            NDWebAppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(email))
                {
                    user.Email = email;
                    user.UserName = email;
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    { }
                    else
                        ModelState.AddModelError("", "E-post kan ikke være tomt");
                }

                if (!string.IsNullOrEmpty(phonenumber))
                {
                    user.PhoneNumber = phonenumber;
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    { }
                    else
                        ModelState.AddModelError("", "Noe gikk galt under oppdatering av tlf.nr.");
                }

                string employeenrString = empnr.ToString();
                if (!string.IsNullOrEmpty(employeenrString))
                {
                    user.empNr = empnr;
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    { }
                    else
                        ModelState.AddModelError("", "Ansattnummer kan ikke være tomt");
                }

                if (!string.IsNullOrEmpty(emplname))
                {
                    user.empLname = emplname;
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    { }
                    else
                        ModelState.AddModelError("", "Etternavn kan ikke være tomt");
                }

                if (!string.IsNullOrEmpty(empfname))
                {
                    user.empFname = empfname;
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded) { 
                        TempData["Message"] = "Bruker " + user.UserName + " (" + user.empFname + " " + user.empLname + ") ble oppdatert.";
                        TempData["Status"] = "Success";
                        return RedirectToAction("Index");
                    }
                    else
                        ModelState.AddModelError("", "Fornavn kan ikke være tomt");
                }

            }
            else
                ModelState.AddModelError("", "Bruker ikke funnet");
            return View(user);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ResetPassword(string id)
        {
            NDWebAppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("NotFound");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id, string password, string confirmPassword)
        {
            NDWebAppUser user = await userManager.FindByIdAsync(id);
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
                            TempData["Message"] = "Passordet til " + user.UserName + " (" + user.empFname + " " + user.empLname + ") ble byttet.";
                            TempData["Status"] = "Success";
                            return RedirectToAction("Index");
                        }
                        else
                            Errors(result);
                    }
                    else
                        ModelState.AddModelError("", "Password cannot be empty");
                }
                else
                    ModelState.AddModelError("", "User Not Found");

            }
            return View(user);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            NDWebAppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData["Message"] = "Brukeren ble slettet";
                    TempData["Status"] = "Success";
                    return RedirectToAction("Index");
                }
                else
                    Errors(result);
                    TempData["Message"] = "Det skjedde noe galt";
                    TempData["Status"] = "Danger";
            }
            else
                ModelState.AddModelError("", "Bruker ikke funnet");
                TempData["Message"] = "Bruker ikke funnet";
                TempData["Status"] = "Danger";
            return View("Index", userManager.Users);
        }

    }
}