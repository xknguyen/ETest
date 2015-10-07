namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "QuestionTitle", c => c.String(nullable: false, maxLength: 200));
            DropColumn("dbo.Questions", "QusetionTitle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "QusetionTitle", c => c.String(nullable: false, maxLength: 200));
            DropColumn("dbo.Questions", "QuestionTitle");
        }
    }
}
