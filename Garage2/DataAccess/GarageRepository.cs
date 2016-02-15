using Garage2.Models;
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

        public IEnumerable<ParkingSlot> ParkingSlots
        {
            get { return db.ParkingSlots; }
        }

        public IEnumerable<Parking> Parkings
        {
            get { return db.Parkings; }
        }

        public IEnumerable<Vehicle> Vehicles
        {
            get { return db.Vehicles; }
        }

        public IEnumerable<Overview> CollatedOverview
        {
            get
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
        }

        private List<Overview> CalcDuration(IEnumerable<Overview> seq)
        {
            var l = seq.ToList();
            for (int i = 0; i < l.Count; ++i)
            {
                l[i].Duration = l[i].DateOut - l[i].DateIn;
            }
            return l;
        }

        public IEnumerable<Overview> FilteredOverview(string searchString, string sortOrder, string TypeFilter, bool? DateinFilter)
        {
            var seq = CollatedOverview;

            if (!String.IsNullOrEmpty(searchString))
            {
                var str = searchString.ToLower();
                seq = seq.Where(s => s.Owner.ToLower().Contains(str) || s.VehicleReg.ToLower().Contains(str));
            }

            if (sortOrder == null)
                sortOrder = "ParkingSlotId";

            bool orderDesc = false;
            if (sortOrder.EndsWith("_desc") && sortOrder.Length > 5)
            {
                sortOrder = sortOrder.Substring(0, sortOrder.Length - 5);
                orderDesc = true;
            }

            var prop = typeof(Overview).GetProperty(sortOrder);
            if (prop != null)
            {
                if (orderDesc)
                    seq = seq.OrderByDescending(x => prop.GetValue(x, null));
                else
                    seq = seq.OrderBy(x => prop.GetValue(x, null));
            }

            if (TypeFilter != null && TypeFilter != "Show All")
            {
                VehicleType vt;
                if (Enum.TryParse(TypeFilter, out vt))
                    seq = seq.Where(s => s.Type == vt);
            }

            if (DateinFilter != null)
            {
                if (DateinFilter == true)
                {
                    //seq = seq.Where(s => s.DateIn.Date.Equals(DateTime.Today.Date)).ToList();
                    seq = seq.Where(s => s.DateIn.Date == DateTime.Today.Date);
                }
            }

            return CalcDuration(seq);
        }

        public bool Park(Vehicle vehicle)
        {
            var item = db.ParkingSlots.FirstOrDefault(i => i.Occupied == false);

            if (item != null)
            {
                item.Occupied = true;

                db.Parkings.Add(new Parking
                {
                    VehicleReg = vehicle.Reg,
                    ParkingSlotId = item.Id,
                    DateIn = DateTime.Today,
                    DateOut = null
                });

                db.Vehicles.Add(vehicle);
                db.SaveChanges();

                return true;
            }

            return false;
        }

        public void Delete(int? id, int? psi, string reg)
        {
            var parkingSlot = db.ParkingSlots.First(i => i.Id == psi);
            parkingSlot.Occupied = false;
            parkingSlot.VehicleReg = null;

            var parking = db.Parkings.First(i => i.Id == id);
            parking.DateOut = DateTime.Now;

            db.SaveChanges();
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