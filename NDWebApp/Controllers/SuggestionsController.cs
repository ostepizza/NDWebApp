using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NDWebApp.Data;
using NDWebApp.Models;
using System.Diagnostics;

namespace NDWebApp.MVC.Controllers
{
    public class SuggestionsController : Controller
    {
        private readonly ISuggestionConnector suggestionConnector;

        public SuggestionsController(ISuggestionConnector suggestionConnector)
        {
            this.suggestionConnector = suggestionConnector;
            suggestionConnector.PopulateStatusInDB();
        }
        public IActionResult Index()
        {
            var data = suggestionConnector.GetSuggestions();
            var model = new SuggestionModel();
            model.Suggestions = data;
            return View(model);
        }

        public IActionResult Add()
        {
            var users = suggestionConnector.GetUsers();
            var teams = suggestionConnector.GetTeams();
            var model = new SuggestionModel();
            model.Users = users;
            model.Teams = teams;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(string SuggestionTitle, string SuggestionDescription, DateTime SuggestionDeadline, string SuggestedUserId, string ResponsibleUserId, int TeamId)
        {
            System.Diagnostics.Debug.WriteLine("Attempts to parse: Title: " + SuggestionTitle + ", Descr: " + SuggestionDescription + ", Deadline: " + SuggestionDeadline + ", Sug. ID: " + SuggestedUserId + ", Res. ID: " + ResponsibleUserId + ", Team ID: " + TeamId);
            int id = suggestionConnector.CreateSuggestion(SuggestionTitle, SuggestionDescription, SuggestionDeadline, SuggestedUserId, ResponsibleUserId, TeamId);
            return RedirectToAction("View", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            suggestionConnector.DeleteSuggestion(id);
            TempData["Message"] = "Forslaget ble slettet";
            TempData["Status"] = "Success";
            return RedirectToAction("Index");
        }

        public IActionResult View(int id)
        {
            var model = suggestionConnector.GetSuggestionById(id);
            return View(model);
        }
    }
}