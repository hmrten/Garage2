using Garage2.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}