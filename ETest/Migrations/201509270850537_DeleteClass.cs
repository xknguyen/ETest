namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteClass : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserProfiles", "ClassId", "dbo.Classes");
            DropIndex("dbo.UserProfiles", new[] { "ClassId" });
            DropColumn("dbo.UserProfiles", "ClassId");
            DropTable("dbo.Classes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        ClassId = c.Long(nullable: false, identity: true),
                        ClassName = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 500),
                        Active = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ClassId);
            
            AddColumn("dbo.UserProfiles", "ClassId", c => c.Long());
            CreateIndex("dbo.UserProfiles", "ClassId");
            AddForeignKey("dbo.UserProfiles", "ClassId", "dbo.Classes", "ClassId");
        }
    }
}
