using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NDWebApp.Models
{
    public class UserModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Phone { get; set; }

        [Required]
        public int empNr { get; set; }

        [Required]
        public string empFname { get; set; } = "";

        [Required]
        public string empLname { get; set; } = "";
    }
}
