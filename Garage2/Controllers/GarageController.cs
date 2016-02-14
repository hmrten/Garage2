﻿using Garage2.DataAccess;
using Garage2.Models;
using Garage2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Garage2.Models;

namespace Garage2.Controllers
{
    public class GarageController : Controller
    {
        const int capacity = 100;

        private GarageContext db = new GarageContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Slots()
        {
            return View(db.ParkingSlots);
        }

        public ActionResult Vehicles()
        {
            return View(db.Vehicles);
        }

        public ActionResult Parkings()
        {
            return View(db.Parkings);
        }

        // GET: Items/Park
        public ActionResult Park()
        {
            return View();
        }

        // POST: Items/Park
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Park([Bind(Include = "Reg,Type,Owner")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                var item = db.ParkingSlots.FirstOrDefault(i => i.Occupied == false);

                if (item != null)
                {
                    db.Vehicles.Add(vehicle);
                    db.SaveChanges();
                }

                return RedirectToAction("Vehicles");
            }

            return View();
        }


		public ActionResult DisplayOverview() 
		{
			var joining = from vehicles in db.Vehicles
						  join parrkings in db.Parkings
						  on vehicles.Reg equals parrkings.VehicleReg
						  select new Overview { VehicleReg = vehicles.Reg, ParkingSlotId = parrkings.ParkingSlotId, DateIn = parrkings.DateIn, DateOut = parrkings.DateOut, Type = vehicles.Type, Owner = vehicles.Owner};
			var durlist = joining.ToList();
			for (int i = 0; i < durlist.Count; i++) 
			{
				durlist[i].Duration = durlist[i].DateOut - durlist[i].DateIn;
			}
				return View(durlist);
		}

        public ActionResult DisplayOverview(string searchString)
        {
            var joining = from vehicles in db.Vehicles
                          join parrkings in db.Parkings
                          on vehicles.Reg equals parrkings.VehicleReg
                          select new Overview { VehicleReg = vehicles.Reg, ParkingSlotId = parrkings.ParkingSlotId, DateIn = parrkings.DateIn, DateOut = parrkings.DateOut, Type = vehicles.Type, Owner = vehicles.Owner };
            var durlist = joining.ToList();

            for (int i = 0; i < durlist.Count; i++)
            {
                durlist[i].Duration = durlist[i].DateOut - durlist[i].DateIn;
            }
            
            if (!String.IsNullOrEmpty(searchString))
            {
                durlist = durlist.Where(s => s.Owner.Contains(searchString));
                durlist = durlist.Where(s => s.VehicleReg.Contains(searchString));
            }

            return View(durlist);
        }
    }
}