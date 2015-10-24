namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDetailTitle : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.QuestionDetails", "QuestionTitle", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.QuestionDetails", "QuestionTitle", c => c.String(nullable: false));
        }
    }
}
