using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NDWebApp.Areas.Identity.Data;
using NDWebApp.Data;
using NDWebApp.Models;
using NDWebApp.Entities;
using System.Diagnostics;
using System.Linq;

namespace NDWebApp.MVC.Controllers
{
    [Authorize]
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
        public IActionResult Add()
        {
            var availableTeams = usersSqlConnector.GetAvailableTeams();
            var model = new UserModel();
            model.AvailableTeams = availableTeams;
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(string email, string phone, string empfname, string emplname, int empnr, int? teamid, string password)
        {
            if (ModelState.IsValid)
            {
                NDWebAppUser appUser = new NDWebAppUser
                {
                    empNr = empnr,
                    empFname = empfname,
                    empLname = emplname,
                    UserName = email,
                    Email = email,
                    PhoneNumber = phone,
                    teamId = teamid
                };

                IdentityResult result = await userManager.CreateAsync(appUser, password);

                if (result.Succeeded) { 
                    TempData["Message"] = "Bruker " + email + " (" + empfname + " " + emplname + ") ble opprettet.";
                    TempData["Status"] = "Success";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View();
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
            // Should be checked better before letting all users search!!
            var searchNN = Convert.ToString(search + "");
            var model = new UserModel(); //M? opprettes for ? ha noe ? returne uansett s?keresultat, i utgangspunktet null
            if (!searchNN.Contains("'") && searchNN != "")
            {
                var data = usersSqlConnector.GetMatchingUsers(search);
                System.Diagnostics.Debug.WriteLine("Attempting to write this: " + data.ToString());
                model = new UserModel();
                model.Users = data;
                //This shit broky and will NEVER return null, can't figure out what else to do instead :DD
                if(data != null)
                {
                    return View(model);
                }
                // Since above is broken, program will never ever get here D:
                TempData["Message"] = "Fant ingen resultater!";
                TempData["Status"] = "Warning";
                return View(model);
            }
            else if (searchNN.Contains("'"))
            {
                //Gir et lite klask p? baken til ansatte som vil pr?ve seg p? spicy symbol i s?kefeltet
                //(Programmet thrower en exception om s?kefeltet inneholder ')
                TempData["Message"] = "Ap-ap-ap! Symbolet du fors?kte ? s?ke med er FORBUDT ? bruke her!";
                TempData["Status"] = "Danger";
                return View(model);
            }
            TempData["Message"] = "S?kefeltet kan ikke v?re tomt!";
            TempData["Status"] = "Warning";
            return View(model);
        }

        [Authorize(Roles = "Administrator,Team Leader")]
        public async Task<IActionResult> View(string id)
        {
            UserModel user = usersSqlConnector.GetUserById(id);
            user.SuggestionsSubmitted = usersSqlConnector.GetUserSuggestions(id);
            user.RepairsSubmitted = usersSqlConnector.GetUserRepairs(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("NotFound");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(string id)
        {
            UserModel user = usersSqlConnector.GetUserById(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("NotFound");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, string email, string phone, string empfname, string emplname, int empnr, int? teamid)
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
                        ModelState.AddModelError("", "E-post kan ikke v?re tomt");
                }

                if (!string.IsNullOrEmpty(phone))
                {
                    user.PhoneNumber = phone;
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
                        ModelState.AddModelError("", "Ansattnummer kan ikke v?re tomt");
                }

                System.Diagnostics.Debug.WriteLine("Supplied team id: "+teamid);
                user.teamId = teamid;
                IdentityResult resultTeamId = await userManager.UpdateAsync(user);
                if (resultTeamId.Succeeded)
                { }
                else
                    ModelState.AddModelError("", "Ansattnummer kan ikke v?re tomt");

                if (!string.IsNullOrEmpty(emplname))
                {
                    user.empLname = emplname;
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    { }
                    else
                        ModelState.AddModelError("", "Etternavn kan ikke v?re tomt");
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
                        ModelState.AddModelError("", "Fornavn kan ikke v?re tomt");
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