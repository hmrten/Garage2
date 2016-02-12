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
            context.Vehicles.AddOrUpdate(v => v.Reg,
                new Vehicle { Reg = "AAA111", Type = VehicleType.Car, Owner = "John Smith" },
                new Vehicle { Reg = "BBB222", Type = VehicleType.Car, Owner = "Joe Forester" },
                new Vehicle { Reg = "CCC333", Type = VehicleType.Truck, Owner = "Bob Alison" },
                new Vehicle { Reg = "DDD444", Type = VehicleType.Bus, Owner = "Alice Carter" },
                new Vehicle { Reg = "EEE555", Type = VehicleType.Motorcycle, Owner = "Eve Dole" }
                );

            if (context.ParkingSlots.Count() == 0)
            {
                for (int i = 0; i < 100; ++i)
                {
                    context.ParkingSlots.AddOrUpdate(new ParkingSlot { Occupied = false, VehicleReg = null });
                }

                context.Parkings.AddOrUpdate(
                    new Parking { VehicleReg = "AAA111", ParkingSlotId = 0, DateIn = new DateTime(2016, 1, 1), DateOut = new DateTime(2016, 1, 2) },
                    new Parking { VehicleReg = "BBB222", ParkingSlotId = 1, DateIn = new DateTime(2016, 1, 10), DateOut = new DateTime(2016, 1, 12) },
                    new Parking { VehicleReg = "CCC333", ParkingSlotId = 2, DateIn = new DateTime(2016, 2, 1), DateOut = new DateTime(2016, 2, 2) },
                    new Parking { VehicleReg = "DDD444", ParkingSlotId = 3, DateIn = new DateTime(2016, 2, 3), DateOut = new DateTime(2016, 2, 4) },
                    new Parking { VehicleReg = "EEE555", ParkingSlotId = 4, DateIn = new DateTime(2016, 2, 5), DateOut = new DateTime(2016, 2, 6) }
                    );
            }

            context.SaveChanges();
        }
    }
}
