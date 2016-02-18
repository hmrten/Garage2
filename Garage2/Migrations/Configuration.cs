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

            context.Owners.AddOrUpdate(o => o.Name,
                new Owner { Name = "Bob" },
                new Owner { Name = "John Smith" },
                new Owner { Name = "Jane Doe" }
                );

            context.SaveChanges();
        }
    }
}
