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

        public TeamModel? TeamId { get; set; }

        public StatusModel? StatusId { get; set; }

        public SuggestionModel()
        {
            
        }
    }
}
