using Microsoft.AspNetCore.Mvc;

namespace NDWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
