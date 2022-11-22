using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NDWebApp.Data;
using NDWebApp.Models;
using System.Diagnostics;

namespace NDWebApp.MVC.Controllers
{
    public class SuggestionsController : Controller
    {
        private readonly ISuggestionRepository suggestionRepository;
        public SuggestionsController(ISuggestionRepository suggestionRepository)
        {
            this.suggestionRepository = suggestionRepository;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            var data = suggestionRepository.GetSuggestions();
            var model = new SuggestionModel();
            model.Suggestions = data;
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