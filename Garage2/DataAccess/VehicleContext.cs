using Garage2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Garage2.DataAccess
{
    public class VehicleContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public VehicleContext() : base("DefaultConnection") { }
    }
}