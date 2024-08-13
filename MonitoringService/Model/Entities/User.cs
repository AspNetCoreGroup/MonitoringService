using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringService.Model.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserID { get; set; }
    }
}