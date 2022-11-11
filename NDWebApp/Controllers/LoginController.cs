using Microsoft.AspNetCore.Mvc;

namespace NDWebApp.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

