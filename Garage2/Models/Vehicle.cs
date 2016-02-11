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
        [Key]
        public int Id { get; set; }
        public VehicleType Type { get; set; }
        public string RegNr { get; set; }
        public string Owner { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime DateOut { get; set; }
    }
}