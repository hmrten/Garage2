namespace Garage2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Owners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Parkings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VehicleReg = c.String(nullable: false, maxLength: 50),
                        ParkingSlotId = c.Int(nullable: false),
                        DateIn = c.DateTime(nullable: false),
                        DateOut = c.DateTime(),
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
                        OwnerId = c.Int(nullable: false),
                        VehicleTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Reg)
                .ForeignKey("dbo.Owners", t => t.OwnerId, cascadeDelete: true)
                .ForeignKey("dbo.VehicleTypes", t => t.VehicleTypeId, cascadeDelete: true)
                .Index(t => t.OwnerId)
                .Index(t => t.VehicleTypeId);
            
            CreateTable(
                "dbo.VehicleTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehicles", "VehicleTypeId", "dbo.VehicleTypes");
            DropForeignKey("dbo.Vehicles", "OwnerId", "dbo.Owners");
            DropIndex("dbo.Vehicles", new[] { "VehicleTypeId" });
            DropIndex("dbo.Vehicles", new[] { "OwnerId" });
            DropTable("dbo.VehicleTypes");
            DropTable("dbo.Vehicles");
            DropTable("dbo.ParkingSlots");
            DropTable("dbo.Parkings");
            DropTable("dbo.Owners");
        }
    }
}
