using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage2.Models
{
    public class ParkingSlot
    {
        [Key]
        public int Id { get; set; }

        public bool Occupied { get; set; }
        
        [StringLength(50), Display(Name = "Registration number")]
        public string VehicleReg { get; set; }
    }
}