namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionTest : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tests", "Question_QuestionId", "dbo.Questions");
            DropIndex("dbo.Tests", new[] { "Question_QuestionId" });
            DropColumn("dbo.Tests", "Question_QuestionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tests", "Question_QuestionId", c => c.Long());
            CreateIndex("dbo.Tests", "Question_QuestionId");
            AddForeignKey("dbo.Tests", "Question_QuestionId", "dbo.Questions", "QuestionId");
        }
    }
}
