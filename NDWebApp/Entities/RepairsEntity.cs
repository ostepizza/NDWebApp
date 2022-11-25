using System.ComponentModel.DataAnnotations.Schema;

namespace NDWebApp.Entities
{
    [Table("Repairs")]
    public class RepairsEntity
    {
        public int RepairId { get; set; }

        public string RepairTitle { get; set; }

        public string RepairDescription { get; set; }

        public DateTime RepairDeadline { get; set; }

        public DateTime RepairEnddate { get; set; }

        public string  UserId { get; set; }

        public string? UserFirstname { get; set; }

        public string? UserLastname { get; set; }

        public int? TeamId { get; set; }

        public string? TeamName { get; set; }

        public int StatusId { get; set; }

        public string? StatusName { get; set; }
    }
}
