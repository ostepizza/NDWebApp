using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NDWebApp.Areas.Identity.Data;
using NDWebApp.Data;
using NDWebApp.Models;
using System.Diagnostics;

namespace NDWebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<NDWebAppUser> _userManager;
        private readonly SignInManager<NDWebAppUser> _signInManager;
        private readonly IHomeSqlConnector homeConnector;

        public HomeController(ILogger<HomeController> logger, IHomeSqlConnector homeConnector, UserManager<NDWebAppUser> userManager,
            SignInManager<NDWebAppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            this.homeConnector = homeConnector;
        }

        [Authorize]
        public IActionResult Index()
        {
            var user = _userManager.GetUserAsync(User).Result.Id;
            var model = homeConnector.GetStatistics(user);
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}