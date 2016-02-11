using Garage2.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Garage2.Controllers
{
    public class VehicleController : Controller
    {
        private VehicleContext db = new VehicleContext();
        
        // GET: Vehicle
        public ActionResult Index()
        {
            return View(db.Vehicles);
        }
    }
}