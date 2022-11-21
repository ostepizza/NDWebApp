using System.ComponentModel.DataAnnotations.Schema;

namespace NDWebApp.Models
{
    public class TeamModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamId { get; set; }

        public string? TeamName { get; set; }

        public string? LeaderEmpNr { get; set; }

        public TeamModel()
        {

        }
    }
}
