using System.ComponentModel.DataAnnotations.Schema;

namespace NDWebApp.Entities
{
    public class TeamMemberEntity
    {
        public int TeamId { get; set; }

        public string? UserId { get; set; }

        public string? empFname { get; set; }

        public string? empLname { get; set; }

    }
}
