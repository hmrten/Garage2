namespace Garage2.Migrations
{
    using Garage2.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Garage2.DataAccess.VehicleContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        private string RandomRegNr(Random rnd)
        {
            var s = new char[6];
            for (int i = 0; i < 3; ++i)
                s[i] = (char)('A' + (rnd.Next() % 26));
            for (int i = 3; i < s.Length; ++i)
                s[i] = (char)('0' + (rnd.Next() % 10));
            return new string(s);
        }

        private string RandomName(Random rnd)
        {
            string[] fnames = { "John", "Joe", "Bob", "Alice", "Eve", "Chris" };
            string[] lnames = { "Smith", "Forester", "Alison", "Carter", "Dole" };
            return fnames[rnd.Next(0, fnames.Length)] + lnames[rnd.Next(0, lnames.Length)];
        }

        private DateTime RandomDate(Random rnd)
        {
            return new DateTime(DateTime.Now.Ticks + rnd.Next(1000000000, 2000000000));
        }

        private VehicleType RandomType(Random rnd)
        {
            return (VehicleType)rnd.Next(0, (int)VehicleType.Last);
        }

        private Vehicle RandomVehicle(Random rnd, int parkingSlotIndex)
        {
            var v = new Vehicle();
            v.RegNr = RandomRegNr(rnd);
            v.Owner = RandomName(rnd);
            v.DateIn = RandomDate(rnd);
            v.Type = RandomType(rnd);
            v.ParkingSlotIndex = parkingSlotIndex;
            return v;
        }

        protected override void Seed(Garage2.DataAccess.VehicleContext context)
        {
            var rnd = new Random();
            for (int i = 0; i < 100; ++i)
            {
                context.Vehicles.AddOrUpdate(v => v.ParkingSlotIndex, RandomVehicle(rnd, i));
            }
            context.SaveChanges();
        }
    }
}
