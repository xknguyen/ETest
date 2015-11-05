namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTestGradeType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tests", "GradeType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tests", "GradeType");
        }
    }
}
