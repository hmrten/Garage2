using Garage2.DataAccess;
using Garage2.Models;
using Garage2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Garage2.Controllers
{
    public class GarageController : Controller
    {
        private GarageRepository repo = new GarageRepository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Slots()
        {
            return View(repo.ParkingSlots);
        }

        public ActionResult Vehicles()
        {
            return View(repo.Vehicles);
        }

        public ActionResult Parkings()
        {
            return View(repo.Parkings);
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
                repo.Park(vehicle);
                return RedirectToAction("Vehicles");
            }
            return View(vehicle);
        }

        public ActionResult DisplayOverview(string searchString, string sortOrder)
        {
            // Default to sorting by parkign slot id
            sortOrder = sortOrder ?? "ParkingSlotId";

            // Update ViewBag
            ViewBag.TypeSortParm = sortOrder == "Type" ? "Type_desc" : "Type";
            ViewBag.OwnSortParm = sortOrder == "Owner" ? "Owner_desc" : "Owner";
            ViewBag.RegSortParm = sortOrder == "VehicleReg" ? "VehicleReg_desc" : "VehicleReg";
            ViewBag.PIDSortParm = sortOrder == "ParkingSlotId" ? "ParkingSlotId_desc" : "ParkingSlotId";
            ViewBag.DateInSortParm = sortOrder == "DateIn" ? "DateIn_desc" : "DateIn";
            ViewBag.DateOutSortParm = sortOrder == "DateOut" ? "DateOut_desc" : "DateOut";
            ViewBag.DurationSortParm = sortOrder == "Duration" ? "Duration_desc" : "Duration";

            var filteredList = repo.FilteredOverview(searchString, sortOrder);

            return View(filteredList);
        }
    }
}