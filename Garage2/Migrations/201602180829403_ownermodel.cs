namespace Garage2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ownermodel : DbMigration
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
            
            AddColumn("dbo.Vehicles", "OwnerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Vehicles", "OwnerId");
            AddForeignKey("dbo.Vehicles", "OwnerId", "dbo.Owners", "Id", cascadeDelete: true);
            DropColumn("dbo.Vehicles", "Owner");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "Owner", c => c.String(nullable: false, maxLength: 100));
            DropForeignKey("dbo.Vehicles", "OwnerId", "dbo.Owners");
            DropIndex("dbo.Vehicles", new[] { "OwnerId" });
            DropColumn("dbo.Vehicles", "OwnerId");
            DropTable("dbo.Owners");
        }
    }
}
