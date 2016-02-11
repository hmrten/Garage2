namespace Garage2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class finalVehicle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "ParkingSlotIndex", c => c.Int(nullable: false));
            DropColumn("dbo.Vehicles", "ParingSlotIndex");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "ParingSlotIndex", c => c.Int(nullable: false));
            DropColumn("dbo.Vehicles", "ParkingSlotIndex");
        }
    }
}
