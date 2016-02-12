using Garage2.DataAccess;
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

    }
}