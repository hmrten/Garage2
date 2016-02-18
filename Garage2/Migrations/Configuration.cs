namespace Garage2.Migrations
{
    using Garage2.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Garage2.DataAccess.GarageContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Garage2.DataAccess.GarageContext context)
        {
            context.VehicleTypes.AddOrUpdate(t => t.Name,
                new VehicleType { Name = "Car" },
                new VehicleType { Name = "Motorcycle" },
                new VehicleType { Name = "Bus" },
                new VehicleType { Name = "Truck" }
                );
            if (context.ParkingSlots.Count() == 0)
            {
                for (int i = 0; i < 100; ++i)
                {
                    context.ParkingSlots.AddOrUpdate(new ParkingSlot { Occupied = false, VehicleReg = null });
                }
            }
            context.SaveChanges();
        }
    }
}
