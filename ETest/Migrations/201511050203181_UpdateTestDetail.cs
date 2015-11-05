namespace ETest.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTestDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TestDetails", "QuestionDetails", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TestDetails", "QuestionDetails");
        }
    }
}
