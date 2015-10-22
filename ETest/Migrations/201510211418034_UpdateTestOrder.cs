namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTestOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TestDetails", "OrderNo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TestDetails", "OrderNo");
        }
    }
}
