using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Garage2.Models
{
    /*public enum VehicleType
    {
        Car,
        Motorcycle,
        Bus,
        Truck
    }*/

    public class Vehicle
    {
        [Key, StringLength(50), Display(Name = "Registration number")]
        public string Reg { get; set; }

        [ForeignKey("OwnerInfo")]
        public int OwnerId { get; set; }
        public virtual Owner OwnerInfo { get; set; }

        [ForeignKey("Type"), Display(Name = "Vehicle Type")]
        public int VehicleTypeId { get; set; }
        public virtual  VehicleType Type { get; set; }
    }
}