using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NDWebApp.Data;
using NDWebApp.Models;
using System.Diagnostics;

namespace NDWebApp.MVC.Controllers
{
    [Authorize]
    public class RepairsController : Controller
    {
        private readonly IRepairsSqlConnector repairConnector;

        public RepairsController(IRepairsSqlConnector repairConnector)
        {
            this.repairConnector = repairConnector;
            repairConnector.PopulateStatusInDB();
        }

        public IActionResult Index()
        {
            var data = repairConnector.GetRepairs();
            var model = new RepairsModel();
            model.Repairs = data;
            return View(model);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(string RepairTitle, string RepairDescription, DateTime RepairDeadline, string UserId, int TeamId)
        {
            System.Diagnostics.Debug.WriteLine("Attempts to parse: Title: " + RepairTitle + ", Descr: " + RepairDescription + ", Deadline: " + RepairDeadline + ", User ID: " + UserId + ", Team ID: " + TeamId);
            int id = repairConnector.CreateRepair(RepairTitle, RepairDescription, RepairDeadline, UserId, TeamId);
            return RedirectToAction("View", new { id = id });
        }

        public IActionResult View(int id)
        {
            var model = repairConnector.GetRepairById(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            repairConnector.DeleteRepair(id);
            TempData["Message"] = "Reparasjonen ble slettet";
            TempData["Status"] = "Success";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Update(int id)
        {
            var users = repairConnector.GetUsers();
            var teams = repairConnector.GetTeams();
            var statusList = repairConnector.GetStatusList();
            RepairsModel model = repairConnector.GetRepairById(id);
            model.Users = users;
            model.Teams = teams;
            model.StatusSelection = statusList;
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int RepairId, string RepairTitle, string RepairDescription, DateTime RepairDeadline, DateTime RepairEnddate, int StatusId)
        {
            System.Diagnostics.Debug.WriteLine("Repair ID: "+RepairId+" ||||||||||||| Date parse attempt: Deadline: " + RepairDeadline + ", Enddate: " + RepairEnddate);
            repairConnector.UpdateRepair(RepairId, RepairTitle, RepairDescription, RepairDeadline, RepairEnddate, StatusId);
            TempData["Message"] = "Forslag " + RepairTitle + " ble oppdatert";
            TempData["Status"] = "Success";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int RepairId, int StatusId)
        {
            repairConnector.UpdateStatus(RepairId, StatusId);
            TempData["Message"] = "Statusen ble oppdatert";
            TempData["Status"] = "Success";
            return RedirectToAction("View", new { id = RepairId });
        }
    }
}