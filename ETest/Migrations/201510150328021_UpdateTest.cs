namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tests", "TestType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tests", "TestType");
        }
    }
}
