namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameQuestionTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionDetails", "QuestionTitle", c => c.String(nullable: false, maxLength: 200));
            DropColumn("dbo.QuestionDetails", "QusetionTitle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuestionDetails", "QusetionTitle", c => c.String(nullable: false, maxLength: 200));
            DropColumn("dbo.QuestionDetails", "QuestionTitle");
        }
    }
}
