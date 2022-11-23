using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace NDWebApp.Models
{
    public class UserCreateModel
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

        [Required]
        public int empNr { get; set; }

        [Required]
        public string empFname { get; set; } = "";

        [Required]
        public string empLname { get; set; } = ""; 

        public IFormFile Photo { get; set; }
    }
}
