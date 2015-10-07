namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateQuestionTitleLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Questions", "QuestionTitle", c => c.String(nullable: false));
            AlterColumn("dbo.QuestionDetails", "QuestionTitle", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.QuestionDetails", "QuestionTitle", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Questions", "QuestionTitle", c => c.String(nullable: false, maxLength: 200));
        }
    }
}
