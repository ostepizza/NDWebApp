using System.ComponentModel.DataAnnotations.Schema;

namespace NDWebApp.Models
{
    public class RepairsModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RepairsId { get; set; }

        public string? RepairsTitle { get; set; }

        public string? RepairsDescription { get; set; }

        public DateTime? RepairsDeadline { get; set; }

        public DateTime? RepairsEnddate { get; set; }

        public string UserId { get; set; }
        
        public int TeamId { get; set; }

        public int StatusId { get; set; }
    }
}
