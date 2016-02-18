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

            context.Owners.AddOrUpdate(o => o.Name,
                new Owner { Name = "Bob" },
                new Owner { Name = "John Smith" },
                new Owner { Name = "Jane Doe" }
                );

            context.SaveChanges();

            if (context.VehicleTypes.Count() > 0)
            {

                context.Vehicles.AddOrUpdate(v => v.Reg,
                    new Vehicle { OwnerId = 1, Reg = "AAA111", OwnerInfo = null, VehicleTypeId = 1 },
                    new Vehicle { OwnerId = 1, Reg = "BBB222", OwnerInfo = null, VehicleTypeId = 2 },
                    new Vehicle { OwnerId = 2, Reg = "CCC333", OwnerInfo = null, VehicleTypeId = 3 },
                    new Vehicle { OwnerId = 3, Reg = "DDD444", OwnerInfo = null, VehicleTypeId = 4 }
                    );

                if (context.Parkings.Count() == 0)
                {
                    context.Parkings.AddOrUpdate(
                        new Parking { ParkingSlotId = 1, VehicleReg = "AAA111", DateIn = new DateTime(2016, 2, 15), DateOut = null },
                        new Parking { ParkingSlotId = 2, VehicleReg = "BBB222", DateIn = new DateTime(2016, 2, 16), DateOut = null },
                        new Parking { ParkingSlotId = 3, VehicleReg = "CCC333", DateIn = new DateTime(2016, 2, 17), DateOut = null },
                        new Parking { ParkingSlotId = 4, VehicleReg = "DDD444", DateIn = new DateTime(2016, 2, 17), DateOut = new DateTime(2016, 2, 18) },
                        new Parking { ParkingSlotId = 5, VehicleReg = "DDD444", DateIn = new DateTime(2016, 2, 18), DateOut = null }
                        );
                    context.ParkingSlots.AddOrUpdate(s => s.Id,
                        new ParkingSlot { Id = 1, Occupied = true, VehicleReg = "AAA111" },
                        new ParkingSlot { Id = 2, Occupied = true, VehicleReg = "BBB222" },
                        new ParkingSlot { Id = 3, Occupied = true, VehicleReg = "CCC333" },
                        new ParkingSlot { Id = 5, Occupied = true, VehicleReg = "DDD444" }
                        );
                }
                /*else
                {
                    context.Parkings.RemoveRange(context.Parkings);
                }*/
            }

            context.SaveChanges();
        }
    }
}
