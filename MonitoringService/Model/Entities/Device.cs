﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringService.Model.Entities
{
    [Table("Devices")]
    public class Device
    {
        [Key]
        public int DeviceID { get; set; }

        public required string DeviceCode { get; set; }
    }
}