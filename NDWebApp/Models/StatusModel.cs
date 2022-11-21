using System.ComponentModel.DataAnnotations.Schema;

namespace NDWebApp.Models
{
    public class StatusModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusId { get; set; }
        public string? StatusTitle { get; set; }
    }
}
