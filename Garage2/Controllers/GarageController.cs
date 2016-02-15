using Garage2.DataAccess;
using Garage2.Models;
using Garage2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Globalization;

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
                    item.Occupied = true;
                    item.VehicleReg = vehicle.Reg;

                    db.Parkings.Add(new Parking
                    {
                        VehicleReg = vehicle.Reg,
                        ParkingSlotId = item.Id,
                        DateIn = DateTime.Today,
                        DateOut = null
                    });

                    db.Vehicles.Add(vehicle);
                    db.SaveChanges();
                }
                return RedirectToAction("DisplayOverview");
            }
            return View();
        }
       
        public ActionResult Unpark()
        {
            var testmodel = db.Parkings
                .Where(i => i.DateOut == null)
                .ToList();

            return View(testmodel);
           
        }

        public ActionResult Delete(int? id, int? psi, string reg)
        {

            var parkingSlot = db.ParkingSlots.First(i => i.Id == psi);
            parkingSlot.Occupied = false;
            parkingSlot.VehicleReg = null;

            var parking = db.Parkings.First(i => i.Id == id);
            parking.DateOut = DateTime.Today;

            //var vehicle = db.Vehicles.First(i => i.Reg == reg);
            //db.Vehicles.Remove(vehicle);

            db.SaveChanges();


            return RedirectToAction("DisplayOverview");
        }

        //public ActionResult DisplayOverview() 
        //{
        //    var joining = from vehicles in db.Vehicles
        //                  join parrkings in db.Parkings
        //                  on vehicles.Reg equals parrkings.VehicleReg
        //                  select new Overview { VehicleReg = vehicles.Reg, ParkingSlotId = parrkings.ParkingSlotId, DateIn = parrkings.DateIn, DateOut = parrkings.DateOut, Type = vehicles.Type, Owner = vehicles.Owner};
        //    var durlist = joining.ToList();
        //    for (int i = 0; i < durlist.Count; i++) 
        //    {
        //        durlist[i].Duration = durlist[i].DateOut - durlist[i].DateIn;
        //    }
        //        return View(durlist);
        //}

		public ActionResult DisplayOverview(string searchString, string sortOrder, string TypeFilter, bool? DateinFilter)
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
				durlist = durlist.Where(s => s.Owner.ToLower().Contains(searchString.ToLower())
					|| s.VehicleReg.ToLower().Contains(searchString.ToLower())).ToList();
            }

            ViewBag.TypeSortParm = sortOrder == "type" ? "type_desc" : "type";
			ViewBag.OwnSortParm = sortOrder == "own" ? "owner_desc" : "own";
			ViewBag.RegSortParm = sortOrder == "VehicleReg" ? "VehicleReg_desc" : "VehicleReg";
			ViewBag.PIDSortParm = sortOrder == "ParkingSlotId" ? "ParkingSlotId_desc" : "ParkingSlotId";
			ViewBag.DateInSortParm = sortOrder == "DateIn" ? "DateIn_desc" : "DateIn";
			ViewBag.DateOutSortParm = sortOrder == "DateOut" ? "DateOut_desc" : "DateOut";
			ViewBag.DurationSortParm = sortOrder == "Duration" ? "Duration_desc" : "Duration";

            switch (sortOrder)
            {
                case "type_desc":
                    durlist = durlist.OrderByDescending(s => s.Type).ToList();
                    break;
                case "type":
                    durlist = durlist.OrderBy(s => s.Type).ToList();
                    break;

				case "owner_desc":
					durlist = durlist.OrderByDescending(s => s.Owner).ToList();
					break;
				case "own":
					durlist = durlist.OrderBy(s => s.Owner).ToList();
					break;

				case "VehicleReg_desc":
					durlist = durlist.OrderByDescending(s => s.VehicleReg).ToList();
					break;
				case "VehicleReg":
					durlist = durlist.OrderBy(s => s.VehicleReg).ToList();
					break;

				case "ParkingSlotId_desc":
					durlist = durlist.OrderByDescending(s => s.ParkingSlotId).ToList();
					break;
				case "ParkingSlotId":
					durlist = durlist.OrderBy(s => s.ParkingSlotId).ToList();
					break;

				case "DateIn_desc":
					durlist = durlist.OrderByDescending(s => s.DateIn).ToList();
					break;
				case "DateIn":
					durlist = durlist.OrderBy(s => s.DateIn).ToList();
					break;
				case "DateOut_desc":
					durlist = durlist.OrderByDescending(s => s.DateOut).ToList();
					break;
				case "DateOut":
					durlist = durlist.OrderBy(s => s.DateOut).ToList();
					break;
				case "Duration_desc":
					durlist = durlist.OrderByDescending(s => s.Duration).ToList();
					break;
				case "Duration":
					durlist = durlist.OrderBy(s => s.Duration).ToList();
					break;
				
            }

			var TypeList = new HashSet<string>();
			
			foreach (var typex in durlist)
			{
				TypeList.Add(typex.Type.ToString());			
			}
			TypeList.Add("Show All");

			ViewBag.TypeList = TypeList;

			
			
			if (TypeFilter != null && TypeFilter != "Show All")
			{
				durlist = durlist.Where(s => s.Type.Equals(Enum.Parse(typeof(VehicleType), TypeFilter))).ToList();
			}

			if (DateinFilter != null)
			{
				if (DateinFilter == true)
				{
					durlist = durlist.Where(s => s.DateIn.Date.Equals(DateTime.Today.Date)).ToList();
				}
			}
			
            
			return View(durlist);
        }
    }
}