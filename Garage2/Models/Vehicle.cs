using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage2.Models
{
    public enum VehicleType
    {
        Car,
        Motorcycle,
        Bus,
        Truck,
        
        Last
    }

    public class Vehicle
    {
        [Key, StringLength(50), Display(Name = "Registration number")]
        public string Reg { get; set; }

        [Required]
        public VehicleType Type { get; set; }

        [Required, StringLength(100)]
        public string Owner { get; set; }
    }
}