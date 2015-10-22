namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTestDecription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tests", "Description", c => c.String(nullable: false));
            DropColumn("dbo.Tests", "TestDescription");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tests", "TestDescription", c => c.String(nullable: false, maxLength: 200));
            DropColumn("dbo.Tests", "Description");
        }
    }
}
