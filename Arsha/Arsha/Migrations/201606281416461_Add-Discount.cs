namespace Arsha.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDiscount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Discount", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Discount");
        }
    }
}
