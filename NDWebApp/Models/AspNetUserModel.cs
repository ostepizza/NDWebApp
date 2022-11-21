using System.ComponentModel.DataAnnotations.Schema;

namespace NDWebApp.Models
{
    public class AspNetUserModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

    }
}
