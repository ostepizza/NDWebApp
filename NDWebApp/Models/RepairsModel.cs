using NDWebApp.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDWebApp.Models
{
    public class RepairsModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RepairId { get; set; }

        public string? RepairTitle { get; set; }

        public string? RepairDescription { get; set; }

        public DateTime RepairDeadline { get; set; }

        public DateTime? RepairEnddate { get; set; }

        public string UserId { get; set; }

        public string? UserFirstname { get; set; }

        public string? UserLastname { get; set; }

        public int? TeamId { get; set; }

        public string? TeamName { get; set; }
        
        public int StatusId { get; set; }

        public string? StatusName { get; set; }

        public IEnumerable<TeamEntity>? Teams { get; set; }

        public IEnumerable<RepairsEntity> Repairs { get; set; }

        public IEnumerable<StatusEntity>? StatusSelection { get; set; }

        public IEnumerable<UserEntity>? Users { get; set; }
    }
}
