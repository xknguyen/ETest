namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUnKnow : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionDetails", "QuestionType", c => c.Int(nullable: false));
            DropColumn("dbo.Questions", "QuestionType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "QuestionType", c => c.Int(nullable: false));
            DropColumn("dbo.QuestionDetails", "QuestionType");
        }
    }
}
