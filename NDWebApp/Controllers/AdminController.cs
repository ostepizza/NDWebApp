using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NDWebApp.Models;
using System.Diagnostics;

namespace NDWebApp.MVC.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}