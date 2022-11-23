using NDWebApp.Entities;
using NDWebApp.MVC.Controllers;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDWebApp.Models
{
    public class SuggestionModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SuggestionId { get; set; }

        public string? SuggestionTitle { get; set; }

        public string? SuggestionDescription { get; set; }

        public DateTime? SuggestionDeadline { get; set;}

        public DateTime? SuggestionEndDate { get; set;}

        public string? SuggestedUserId { get; set; }

        public string? ResponsibleUserId { get; set; }
        
        public int TeamId { get; set; }

        public int StatusId { get; set; }

        
        public IEnumerable<SuggestionEntity> Suggestions { get; set; }

    }
}
