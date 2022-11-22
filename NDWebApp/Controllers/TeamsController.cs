using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NDWebApp.Models;
using NDWebApp.Data;

using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using NDWebApp.Areas.Identity.Data;

namespace NDWebApp.MVC.Controllers
{
    public class TeamsController : Controller
    {
        private ILogger<TeamsController> _logger;
        private readonly ISqlConnector teamSqlConnector;
        
        public TeamsController(ILogger<TeamsController> logger, ISqlConnector teamSqlConnector)
        {
            _logger = logger;
            this.teamSqlConnector = teamSqlConnector;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            var data = teamSqlConnector.GetTeams();
            var model = new TeamModel();
            model.Teams = data;
            return View(model);
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult View()
        {
            return View();
        }
    }
}