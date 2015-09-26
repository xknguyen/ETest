namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Account : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserProfiles", "Identity", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfiles", "Identity", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
