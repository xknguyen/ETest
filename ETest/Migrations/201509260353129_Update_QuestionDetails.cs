namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_QuestionDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionDetails",
                c => new
                    {
                        QuestionDetailId = c.Long(nullable: false, identity: true),
                        QusetionTitle = c.String(nullable: false, maxLength: 200),
                        Choice = c.String(),
                        Answer = c.String(),
                        OrderNo = c.Int(nullable: false),
                        QuestionId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionDetailId)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            DropColumn("dbo.Questions", "Choice");
            DropColumn("dbo.Questions", "Answer");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "Answer", c => c.String());
            AddColumn("dbo.Questions", "Choice", c => c.String(nullable: false));
            DropForeignKey("dbo.QuestionDetails", "QuestionId", "dbo.Questions");
            DropIndex("dbo.QuestionDetails", new[] { "QuestionId" });
            DropTable("dbo.QuestionDetails");
        }
    }
}
