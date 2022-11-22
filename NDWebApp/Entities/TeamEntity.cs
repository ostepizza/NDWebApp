using System.ComponentModel.DataAnnotations.Schema;

namespace NDWebApp.Entities
{
    public class TeamEntity
    {
        public int TeamId { get; set; }

        public string? TeamName { get; set; }

        public string?  LeaderUserId { get; set; }

        public string? Id { get; set; }

        public string? empFname { get; set; }

        public string? empLname { get; set; }
    }
}
