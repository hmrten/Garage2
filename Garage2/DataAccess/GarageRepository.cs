using Garage2.Models;
using Garage2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public IEnumerable<VehicleType> VehicleTypes
        {
            get { return db.VehicleTypes; }
        }

        public IEnumerable<Owner> Owners
        {
            get { return db.Owners; }
        }

        public IEnumerable<Overview> CollatedOverview
        {
            get
            {
				var seq = from v in db.Vehicles
                          join p in db.Parkings
                          on v.Reg equals p.VehicleReg
                          join o in db.Owners
						  on v.OwnerId equals o.Id
						  join t in db.VehicleTypes
						  on v.VehicleTypeId equals t.Id
						  select new Overview
						  {
							  VehicleReg = p.VehicleReg,
							  ParkingSlotId = p.ParkingSlotId,
							  DateIn = p.DateIn,
							  DateOut = p.DateOut,
							  Type = t.Name,
							  OwnerName = o.Name					  
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

        public IEnumerable<Overview> FilteredOverview(string searchString, string sortOrder, string TypeFilter, bool? DateinFilter, bool? VehFilter)
        {
            var seq = CollatedOverview;

            if (!String.IsNullOrEmpty(searchString))
            {
                var str = searchString.ToLower();
                seq = seq.Where(s => s.OwnerName.ToLower().Equals(str) || s.VehicleReg.ToLower().Equals(str));
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


            if (DateinFilter != null)
            {
                if (DateinFilter == true)
                {
                    //seq = seq.Where(s => s.DateIn.Date.Equals(DateTime.Today.Date)).ToList();
                    seq = seq.Where(s => s.DateIn.Date == DateTime.Today.Date);
                }
            }

			if (VehFilter != null) 
			{
				if (VehFilter== true)
				{
                    if (TypeFilter != null)
                    {
                        var name = db.VehicleTypes.Find(int.Parse(TypeFilter)).Name;
                        seq = seq.Where(o => o.Type == name);
                        //seq = seq.Where(s => s.Type == vt);
                    }
					//seq = seq.Where(s => s.Type == TypeFilter.ToString());
				}
			}

			return CalcDuration(seq);

        }

        public bool Register(Vehicle vehicle)
        {

            db.Vehicles.Add(vehicle);
            db.SaveChanges();

            return true;
        }

        public int RegOwner(Owner owner)
        {

            db.Owners.Add(owner);
            db.SaveChanges();

            return owner.Id;
        }

        public bool Park(String reg)
        {
            var item = db.Vehicles.FirstOrDefault(i => i.Reg == reg);

            var item2 = db.ParkingSlots.FirstOrDefault(i => i.Occupied == false);

            bool parked = db.ParkingSlots.Any(i => i.VehicleReg == reg);

            if (parked)
                return true;
	        
            if (item != null && item2 != null)
            {
                item2.Occupied = true;
                item2.VehicleReg = reg;

                db.Parkings.Add(new Parking
                {
                    VehicleReg = reg,
                    ParkingSlotId = item2.Id,
                    DateIn = DateTime.Now,
                    DateOut = null
                });

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

        public IEnumerable<SelectListItem> GetTypeList()
        {
            return (from t in db.VehicleTypes
                    select new SelectListItem() { Text = t.Name, Value = t.Id.ToString() }).ToList();
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