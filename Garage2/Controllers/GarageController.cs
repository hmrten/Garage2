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

        public ActionResult Owners()
        {
            return View(repo.Owners);
        }

        //// GET: Items/RegOwner
        //public ActionResult RegOwner()
        //{
        //    return View();
        //}

        //// POST: Items/RegOwner
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult RegOwner([Bind(Include = "Name")] Owner owner)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        repo.RegOwner(owner);
        //        return RedirectToAction("Register");
        //    }
        //    return View();
        //}


        // GET: Items/Register
        public ActionResult Register()
        {
            ViewBag.TypeList = repo.GetTypeList();
            return View();
        }


        // POST: Items/Register
        [HttpPost]
        
        public ActionResult Register(string reg, int vehicleTypeId, string name)
        {

            int ownId = repo.RegOwner(new Owner { Name = name });

            repo.Register(new Vehicle { OwnerId = ownId, Reg = reg, OwnerInfo = null, VehicleTypeId = vehicleTypeId });
            return RedirectToAction("Park");
         
            //return View();
        }




        //// POST: Items/Register
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Register([Bind(Include = "Reg, VehicleTypeId, OwnerId")] Vehicle vehicle)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        repo.Register(vehicle);
        //        return RedirectToAction("Vehicles");
        //    }
        //    return View(vehicle);
        //}




        // GET: Items/Park
        public ActionResult Park()
        {
            return View();
        }

        // POST: Items/Park
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Park([Bind(Include = "Reg")] String reg)
        {
            if (ModelState.IsValid)
            {
                if (!repo.Park(reg))
                    return RedirectToAction("Register");
            }
            return RedirectToAction("Slots");
        }


        //// POST: Items/Park
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Park([Bind(Include = "Reg,Type,Owner")] Vehicle vehicle)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        repo.Park(vehicle);
        //        return RedirectToAction("Vehicles");
        //    }
        //    return View(vehicle);
        //}

        public ActionResult Unpark()
        {
            var testmodel = repo.Parkings
                .Where(i => i.DateOut == null)
                .ToList();

            return View(testmodel);
           
        }

        public ActionResult Delete(int? id, int? psi, string reg)
        {
            repo.Delete(id, psi, reg);
            return RedirectToAction("DisplayOverview");
        }

		public ActionResult DisplayOverview(string searchString, string sortOrder, string TypeFilter, bool? DateinFilter, bool? VehFilter)
        {
            // Default to sorting by parking slot id
            sortOrder = sortOrder ?? "ParkingSlotId";

            // Update ViewBag
            ViewBag.TypeSortParm = sortOrder == "Type" ? "Type_desc" : "Type";
            ViewBag.OwnSortParm = sortOrder == "Owner" ? "Owner_desc" : "Owner";
			ViewBag.RegSortParm = sortOrder == "VehicleReg" ? "VehicleReg_desc" : "VehicleReg";
			ViewBag.PIDSortParm = sortOrder == "ParkingSlotId" ? "ParkingSlotId_desc" : "ParkingSlotId";
			ViewBag.DateInSortParm = sortOrder == "DateIn" ? "DateIn_desc" : "DateIn";
			ViewBag.DateOutSortParm = sortOrder == "DateOut" ? "DateOut_desc" : "DateOut";
			ViewBag.DurationSortParm = sortOrder == "Duration" ? "Duration_desc" : "Duration";


            /*var TypeList = new List<string>();

			TypeList.Add("Show All");
			foreach (var typex in Enum.GetValues(typeof(VehicleType)))
            {
                TypeList.Add(typex.ToString());
            }

            ViewBag.TypeList = TypeList;*/

            ViewBag.TypeList = repo.GetTypeList();

            var filteredList = repo.FilteredOverview(searchString, sortOrder, TypeFilter, DateinFilter, VehFilter);

            return View(filteredList);
        }
    }
}