using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NDWebApp.Models;
using NDWebApp.Data;

using System.Diagnostics;

namespace NDWebApp.MVC.Controllers
{
    public class TeamsController : Controller
    {
        private ILogger<TeamsController> _logger;
        private readonly ISqlConnector sqlConnector;
        
        public TeamsController(ILogger<TeamsController> logger, ISqlConnector sqlConnector)
        {
            _logger = logger;
            this.sqlConnector = sqlConnector;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            var data = sqlConnector.GetTeams();
            var model = new TeamModel();
            model.Teams = data;
            return View("Teams", model);
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult Detailed()
        {
            return View();
        }
    }
}