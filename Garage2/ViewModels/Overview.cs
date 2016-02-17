using Garage2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage2.ViewModels
{
	public class Overview
	{

		[Required, StringLength(50), Display(Name = "Registration number")]
		public string VehicleReg { get; set; }

		[Required]
		public int ParkingSlotId { get; set; }

		[Display(Name = "Date in")]
		public DateTime DateIn { get; set; }

		[Display(Name = "Date out")]
		public DateTime? DateOut { get; set; }
		
		[Required, Display(Name ="Vehicle Type")]
		public string Type { get; set; }

		[Required, StringLength(100)]
		public string Owner { get; set; }

		public TimeSpan? Duration { set; get; }
	}
}