namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTestTitle : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tests", "TestTitle", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tests", "TestTitle", c => c.String(nullable: false, maxLength: 200));
        }
    }
}
