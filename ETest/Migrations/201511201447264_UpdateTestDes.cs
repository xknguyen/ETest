namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTestDes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tests", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tests", "Description", c => c.String(nullable: false));
        }
    }
}
