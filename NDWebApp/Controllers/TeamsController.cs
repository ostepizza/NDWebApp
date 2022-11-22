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
        private readonly ITeamSqlConnector teamSqlConnector;
        
        public TeamsController(ILogger<TeamsController> logger, ITeamSqlConnector teamSqlConnector)
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

        public IActionResult Detailed()
        {
            return View();
        }
    }
}