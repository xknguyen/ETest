namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateGroupLenght : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Groups", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Groups", "Description", c => c.String(maxLength: 1000));
        }
    }
}
