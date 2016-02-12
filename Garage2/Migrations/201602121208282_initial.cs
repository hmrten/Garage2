namespace Garage2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Parkings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VehicleReg = c.String(nullable: false, maxLength: 50),
                        ParkingSlotId = c.Int(nullable: false),
                        DateIn = c.DateTime(nullable: false),
                        DateOut = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParkingSlots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Occupied = c.Boolean(nullable: false),
                        VehicleReg = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Reg = c.String(nullable: false, maxLength: 50),
                        Type = c.Int(nullable: false),
                        Owner = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Reg);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Vehicles");
            DropTable("dbo.ParkingSlots");
            DropTable("dbo.Parkings");
        }
    }
}
