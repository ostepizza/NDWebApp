using System.ComponentModel.DataAnnotations.Schema;
using NDWebApp.Entities;

namespace NDWebApp.Models
{
    public class TeamModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamId { get; set; }

        public string? TeamName { get; set; }

        public string? LeaderUserId { get; set; }

        public IEnumerable<TeamEntity> Teams { get; set; }

    }
}
