namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTestInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tests", "TestEnd", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tests", "SubmitNo", c => c.Int(nullable: false));
            AddColumn("dbo.Tests", "Score", c => c.Int(nullable: false));
            AddColumn("dbo.Tests", "MixedQuestions", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tests", "MixedQuestions");
            DropColumn("dbo.Tests", "Score");
            DropColumn("dbo.Tests", "SubmitNo");
            DropColumn("dbo.Tests", "TestEnd");
        }
    }
}
