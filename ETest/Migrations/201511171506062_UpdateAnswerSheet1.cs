namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAnswerSheet1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnswerSheets", "IsDone", c => c.Boolean(nullable: false));
            AddColumn("dbo.AnswerSheets", "Score", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AnswerSheets", "Score");
            DropColumn("dbo.AnswerSheets", "IsDone");
        }
    }
}
