using System.ComponentModel.DataAnnotations.Schema;

namespace NDWebApp.Entities
{
    [Table("Repairs")]
    public class RepairsEntity
    {
        public int RepairsId { get; set; }

        public string RepairsTitle { get; set; }

        public string RepairsDescription { get; set; }

        public DateTime RepairsDeadline { get; set; }

        public DateTime RepairsEnddate { get; set; }

        public string  UserId { get; set; }

        public string? UserFirstname { get; set; }

        public string? UserLastname { get; set; }

        public int TeamId { get; set; }

        public string? TeamName { get; set; }

        public int StatusId { get; set; }

        public string? StatusName { get; set; }
    }
}
