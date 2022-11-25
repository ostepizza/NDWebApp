using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NDWebApp.Models;
using NDWebApp.Data;

using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using NDWebApp.Areas.Identity.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NDWebApp.MVC.Controllers
{
    [Authorize]
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

        [Authorize(Roles = "Administrator")]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(string teamName, string leaderUserId)
        {
            teamSqlConnector.CreateTeam(teamName, leaderUserId);

            TempData["Message"] = "Team " + teamName + " ble opprettet.";
            TempData["Status"] = "Success";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            teamSqlConnector.DeleteTeam(id);
            TempData["Message"] = "Teamet ble slettet";
            TempData["Status"] = "Success";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Update(int id)
        {
            return View(id);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, string teamName, string leaderUserId)
        {
            System.Diagnostics.Debug.WriteLine(id + " " + teamName + " " + leaderUserId);
            teamSqlConnector.UpdateTeam(id, teamName, leaderUserId);
            TempData["Message"] = "Team " + teamName + " ble oppdatert";
            TempData["Status"] = "Success";
            return RedirectToAction("Index");
        }

        public IActionResult View(int id)
        {
            var teamMembers = teamSqlConnector.GetTeamMembers(id);
            var model = teamSqlConnector.GetTeamById(id);
            model.TeamMembers = teamMembers;
            return View(model);
        }
    }
}