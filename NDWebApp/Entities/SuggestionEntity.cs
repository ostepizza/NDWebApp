using System.ComponentModel.DataAnnotations.Schema;

namespace NDWebApp.Entities
{
    [Table("Suggestion")]
    public class SuggestionEntity
    {
        public int SuggestionId { get; set; }

        public string? SuggestionTitle { get; set; }

        public string? SuggestionDescription { get; set; }

        public DateTime? SuggestionDeadline { get; set;}

        public DateTime? SuggestionEnddate { get; set; }

        public string? SuggestedUserId { get; set; }

        public string? ResponsibleUserId { get; set; }

        public string? StatusId { get; set; }

        public TeamEntity? TeamId { get; set; }



    }
}
