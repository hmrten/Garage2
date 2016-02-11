namespace Garage2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vehicle2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Vehicles", "DateOut");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "DateOut", c => c.DateTime(nullable: false));
        }
    }
}
