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

        public DateTime SuggestionDeadline { get; set;}

        public DateTime? SuggestionEndDate { get; set;}

        public string? SuggestedUserId { get; set; }

        public string? SuggestedFirstname { get; set; }

        public string? SuggestedLastname { get; set; }

        public string? ResponsibleUserId { get; set; }

        public string? ResponsibleFirstname { get; set; }

        public string? ResponsibleLastname { get; set; }

        public int? TeamId { get; set; }

        public string? TeamName { get; set; }

        public int StatusId { get; set; }

        public string? StatusName { get; set; }

        public IEnumerable<UserEntity>? Users { get; set; }

        public IEnumerable<TeamEntity>? Teams { get; set; }

        public IEnumerable<StatusEntity>? StatusSelection { get; set; }

        public IEnumerable<SuggestionEntity> Suggestions { get; set; }

    }
}
