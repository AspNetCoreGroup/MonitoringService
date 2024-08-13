using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringService.Model.Entities
{
    [Table("Networks")]
    public class Network
    {
        [Key]
        public int NetworkID { get; set; }

        public List<NetworkUser>? NetworkUsers { get; set; }

        public List<NetworkDevice>? NetworkDevices { get; set; }
    }
}