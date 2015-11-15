namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAnswerSheet : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Answers", "AnswerSheetId", "dbo.AnswerSheets");
            DropIndex("dbo.Answers", new[] { "AnswerSheetId" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            AddColumn("dbo.AnswerSheets", "Answer", c => c.String());
            DropTable("dbo.Answers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        AnswerSheetId = c.Long(nullable: false),
                        QuestionId = c.Long(nullable: false),
                        AnswerString = c.String(nullable: false, storeType: "ntext"),
                    })
                .PrimaryKey(t => new { t.AnswerSheetId, t.QuestionId });
            
            DropColumn("dbo.AnswerSheets", "Answer");
            CreateIndex("dbo.Answers", "QuestionId");
            CreateIndex("dbo.Answers", "AnswerSheetId");
            AddForeignKey("dbo.Answers", "AnswerSheetId", "dbo.AnswerSheets", "AnswerSheetId", cascadeDelete: true);
            AddForeignKey("dbo.Answers", "QuestionId", "dbo.Questions", "QuestionId", cascadeDelete: true);
        }
    }
}
