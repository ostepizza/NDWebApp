using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NDWebApp.Models;
using System.Diagnostics;

namespace NDWebApp.MVC.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }
    }
}