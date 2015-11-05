namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTestProperti : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tests", "TeacherId", "dbo.Accounts");
            DropIndex("dbo.Tests", new[] { "TeacherId" });
            DropColumn("dbo.Tests", "TeacherId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tests", "TeacherId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Tests", "TeacherId");
            AddForeignKey("dbo.Tests", "TeacherId", "dbo.Accounts", "Id");
        }
    }
}
