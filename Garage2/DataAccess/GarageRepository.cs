using Garage2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage2.DataAccess
{
    public class GarageRepository : IDisposable
    {
        private GarageContext db = new GarageContext();

        private IEnumerable<Overview> CollateOverview()
        {
            var seq = from vehicles in db.Vehicles
                      join parrkings in db.Parkings
                      on vehicles.Reg equals parrkings.VehicleReg
                      select new Overview
                      {
                          VehicleReg = vehicles.Reg,
                          ParkingSlotId = parrkings.ParkingSlotId,
                          DateIn = parrkings.DateIn,
                          DateOut = parrkings.DateOut,
                          Type = vehicles.Type,
                          Owner = vehicles.Owner
                      };
            return seq;
        }

        public IEnumerable<Overview> FilteredOverview(string searchString, string sortOrder)
        {
            var seq = CollateOverview();

            if (!String.IsNullOrEmpty(searchString))
            {
                seq = seq.Where(s => s.Owner.Contains(searchString) || s.VehicleReg.Contains(searchString));
            }

            //IOrderedEnumerable<Overview> o;

            var f = Type.GetType("Garage2.ViewModels.Overview").GetProperty("Type");

            switch (sortOrder)
            {
                case "type_desc": seq = seq.OrderByDescending(s => f); break;

                case "owner_desc": seq = seq.OrderByDescending(s => s.Owner); break;
                case "own": seq = seq.OrderBy(s => s.Owner); break;

                case "VehicleReg_desc": seq = seq.OrderByDescending(s => s.VehicleReg); break;
                case "VehicleReg": seq = seq.OrderBy(s => s.VehicleReg); break;

                case "ParkingSlotId_desc": seq = seq.OrderByDescending(s => s.ParkingSlotId); break;
                case "ParkingSlotId": seq = seq.OrderBy(s => s.ParkingSlotId); break;

                case "DateIn_desc": seq = seq.OrderByDescending(s => s.DateIn); break;
                case "DateIn": seq = seq.OrderBy(s => s.DateIn); break;
                case "DateOut_desc": seq = seq.OrderByDescending(s => s.DateOut); break;
                case "DateOut": seq = seq.OrderBy(s => s.DateOut); break;
                case "Duration_desc": seq = seq.OrderByDescending(s => s.Duration); break;
                case "Duration": seq = seq.OrderBy(s => s.Duration); break;

                default:
                    seq = seq.OrderBy(s => s.Type); break;
            }

            return seq;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}