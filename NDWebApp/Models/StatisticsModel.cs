using NDWebApp.Entities;
using NDWebApp.MVC.Controllers;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDWebApp.Models
{
    public class StatisticsModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SuggestionsAllUnderVurdering { get; set; }

        public int SuggestionsAllGodtatt { get; set; }

        public int SuggestionsAllAvslatt { get; set; }

        public int SuggestionsAllPagar { get; set; }

        public int SuggestionsAllPaPause { get; set; }

        public int SuggestionsAllFerdig { get; set; }

        public int SuggestionsAllCount { get; set; }

        public int SuggestionsUnderVurdering { get; set; }

        public int SuggestionsGodtatt { get; set; }

        public int SuggestionsAvslatt { get; set; }

        public int SuggestionsPagar { get; set; }

        public int SuggestionsPaPause { get; set; }

        public int SuggestionsFerdig { get; set; }

        public int SuggestionsCount { get; set; }

        public string UserId { get; set; }

    }
}
