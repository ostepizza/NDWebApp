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
    }
}