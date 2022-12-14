using NDWebApp.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NDWebApp.Models
{
    public class UserModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Phone { get; set; } = "";

        public string Id { get; set; }

        [Required]
        public int empNr { get; set; }

        [Required]
        public string empFname { get; set; } = "";

        [Required]
        public string empLname { get; set; } = "";

        [RegularExpression(@"^[^\\/:*;\.\)\(]+$", ErrorMessage = "Symbolene ':', '.' ';', '*', '/' og '\' er ikke tillatt")]
        public string? search { get; set; }

        public int? teamId { get; set; }

        public string? teamName { get; set; }

        public IEnumerable<UserEntity>? Users { get; set; }

        public IEnumerable<TeamEntity>? AvailableTeams { get; set; }

        public IEnumerable<SuggestionEntity>? SuggestionsSubmitted { get; set; }

        public IEnumerable<RepairsEntity>? RepairsSubmitted { get; set; }
    }
}
