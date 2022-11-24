using System.ComponentModel.DataAnnotations.Schema;

namespace NDWebApp.Entities
{
    [Table("Suggestion")]
    public class SuggestionEntity
    {
        public int SuggestionId { get; set; }

        public string? SuggestionTitle { get; set; }

        public string? SuggestionDescription { get; set; }

        public DateTime? SuggestionDeadline { get; set; }

        public DateTime? SuggestionEnddate { get; set; }

        public string? SuggestedUserId { get; set; }

        public string? SuggestedFirstname { get; set; }

        public string? SuggestedLastname { get; set; }

        public string? ResponsibleUserId { get; set; }

        public string? ResponsibleFirstname { get; set; }

        public string? ResponsibleLastname { get; set; }

        public int? TeamId { get; set; }

        public string? TeamName { get; set; }

        public int? StatusId { get; set; }

        public string? StatusName { get; set; }
    }
}