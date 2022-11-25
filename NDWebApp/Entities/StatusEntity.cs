using System.ComponentModel.DataAnnotations.Schema;

namespace NDWebApp.Entities
{
    [Table("Status")]
    public class StatusEntity
    {
        public int StatusId { get; set; }

        public string StatusTitle { get; set; }
    }
}
