namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Actived : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Actived", c => c.Boolean(nullable: false));
            AddColumn("dbo.Groups", "Actived", c => c.Boolean(nullable: false));
            AddColumn("dbo.Groups", "OrderNo", c => c.Int(nullable: false));
            AddColumn("dbo.Tests", "Actived", c => c.Boolean(nullable: false));
            AddColumn("dbo.Courses", "Actived", c => c.Boolean(nullable: false));
            DropColumn("dbo.Questions", "Active");
            DropColumn("dbo.Groups", "Active");
            DropColumn("dbo.Tests", "Active");
            DropColumn("dbo.Courses", "Active");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tests", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Groups", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Questions", "Active", c => c.Boolean(nullable: false));
            DropColumn("dbo.Courses", "Actived");
            DropColumn("dbo.Tests", "Actived");
            DropColumn("dbo.Groups", "OrderNo");
            DropColumn("dbo.Groups", "Actived");
            DropColumn("dbo.Questions", "Actived");
        }
    }
}
