using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Garage2.Models
{
    public class Parking
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50), Display(Name = "Registration number")]
        public string VehicleReg { get; set; }

        [Required]
        public int ParkingSlotId { get; set; }

        [Display(Name = "Date in")]
        public DateTime DateIn { get; set; }

        [Display(Name = "Date out")]
        public DateTime? DateOut { get; set; }
    }
}