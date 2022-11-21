using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NDWebApp.Areas.Identity.Data;
using NDWebApp.Models;
using System.Diagnostics;

namespace NDWebApp.MVC.Controllers
{
    public class UsersController : Controller
    {
        private UserManager<NDWebAppUser> userManager;
        private IPasswordHasher<NDWebAppUser> passwordHasher;

        public UsersController(UserManager<NDWebAppUser> usrMgr, IPasswordHasher<NDWebAppUser> passwordHash)
        {
            userManager = usrMgr;
            passwordHasher = passwordHash;
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

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult NotFound()
        {
            return View();
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
        [HttpPost]
        public async Task<IActionResult> Update(string id, string email, string phonenumber, string empfname, string emplname, int empnr)
        {
            NDWebAppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(email))
                    user.Email = email;
<<<<<<< Updated upstream
                else
                    ModelState.AddModelError("", "Email cannot be empty");
=======
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    { }
                    else
                        ModelState.AddModelError("", "Email cannot be empty");
                }

                if (!string.IsNullOrEmpty(phonenumber))
                {
                    user.PhoneNumber = phonenumber;
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    { }
                    else
                        ModelState.AddModelError("", "Email cannot be empty");
                }

                string employeenrString = empnr.ToString();
                if (!string.IsNullOrEmpty(employeenrString))
                {
                    user.empNr = empnr;
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    { }
                    else
                        ModelState.AddModelError("", "Employee number cannot be empty");
                }

                if (!string.IsNullOrEmpty(emplname))
                {
                    user.empLname = emplname;
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    { }
                    else
                        ModelState.AddModelError("", "Lname cannot be empty");
                }

                if (!string.IsNullOrEmpty(empfname))
                {
                    user.empFname = empfname;
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        ModelState.AddModelError("", "Fname cannot be empty");
                }

            }
            else
                ModelState.AddModelError("", "User Not Found");
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
>>>>>>> Stashed changes

                if (!string.IsNullOrEmpty(password))
                    user.PasswordHash = passwordHasher.HashPassword(user, password);
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            NDWebAppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Success");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("Index", userManager.Users);
        }

    }
}