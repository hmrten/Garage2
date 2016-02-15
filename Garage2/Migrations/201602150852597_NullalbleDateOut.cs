namespace Garage2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullalbleDateOut : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Parkings", "DateOut", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Parkings", "DateOut", c => c.DateTime(nullable: false));
        }
    }
}
