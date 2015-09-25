namespace ETest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Intial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AnswerSheets",
                c => new
                    {
                        AnswerSheetId = c.Long(nullable: false, identity: true),
                        TestId = c.Long(nullable: false),
                        TestTakerId = c.String(nullable: false, maxLength: 128),
                        SubmitTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AnswerSheetId)
                .ForeignKey("dbo.Tests", t => t.TestId, cascadeDelete: true)
                .ForeignKey("dbo.Accounts", t => t.TestTakerId, cascadeDelete: true)
                .Index(t => t.TestId)
                .Index(t => t.TestTakerId);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        AnswerSheetId = c.Long(nullable: false),
                        QuestionId = c.Long(nullable: false),
                        AnswerString = c.String(nullable: false, storeType: "ntext"),
                    })
                .PrimaryKey(t => new { t.AnswerSheetId, t.QuestionId })
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.AnswerSheets", t => t.AnswerSheetId, cascadeDelete: true)
                .Index(t => t.AnswerSheetId)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionId = c.Long(nullable: false, identity: true),
                        QusetionTitle = c.String(nullable: false, maxLength: 200),
                        Choice = c.String(nullable: false),
                        Answer = c.String(),
                        GroupId = c.Long(nullable: false),
                        Active = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        QuestionType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionId)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupId = c.Long(nullable: false, identity: true),
                        GroupName = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 1000),
                        ParentGroupId = c.Long(),
                        TeacherId = c.String(nullable: false, maxLength: 128),
                        Active = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.GroupId)
                .ForeignKey("dbo.Groups", t => t.ParentGroupId)
                .ForeignKey("dbo.Accounts", t => t.TeacherId)
                .Index(t => t.ParentGroupId)
                .Index(t => t.TeacherId);
            
            CreateTable(
                "dbo.TestDetails",
                c => new
                    {
                        TestId = c.Long(nullable: false),
                        QuestionId = c.Long(nullable: false),
                        Score = c.Single(nullable: false),
                    })
                .PrimaryKey(t => new { t.TestId, t.QuestionId })
                .ForeignKey("dbo.Tests", t => t.TestId, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.TestId)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        TestId = c.Long(nullable: false, identity: true),
                        TestTitle = c.String(nullable: false, maxLength: 200),
                        TestDescription = c.String(nullable: false, maxLength: 200),
                        TestStart = c.DateTime(nullable: false),
                        TestTime = c.Int(nullable: false),
                        CourseId = c.Long(nullable: false),
                        TeacherId = c.String(maxLength: 128),
                        Active = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Question_QuestionId = c.Long(),
                    })
                .PrimaryKey(t => t.TestId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Accounts", t => t.TeacherId)
                .ForeignKey("dbo.Questions", t => t.Question_QuestionId)
                .Index(t => t.CourseId)
                .Index(t => t.TeacherId)
                .Index(t => t.Question_QuestionId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Long(nullable: false, identity: true),
                        CourseName = c.String(nullable: false, maxLength: 200),
                        Description = c.String(maxLength: 1000),
                        TeacherId = c.String(nullable: false, maxLength: 128),
                        Active = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CourseId)
                .ForeignKey("dbo.Accounts", t => t.TeacherId)
                .Index(t => t.TeacherId);
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Accounts", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        AccountId = c.String(nullable: false, maxLength: 128),
                        Identity = c.String(nullable: false, maxLength: 10),
                        LastName = c.String(nullable: false, maxLength: 100),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        Notes = c.String(maxLength: 1000),
                        ClassId = c.Long(),
                        BirthDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AccountId)
                .ForeignKey("dbo.Accounts", t => t.AccountId)
                .ForeignKey("dbo.Classes", t => t.ClassId)
                .Index(t => t.AccountId)
                .Index(t => t.ClassId);
            
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
            
            CreateTable(
                "dbo.UserInRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Accounts", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.CourseStudent",
                c => new
                    {
                        CourseId = c.Long(nullable: false),
                        AccountId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CourseId, t.AccountId })
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.CourseId)
                .Index(t => t.AccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserInRoles", "UserId", "dbo.Accounts");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Accounts");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Accounts");
            DropForeignKey("dbo.UserInRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserProfiles", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.UserProfiles", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Groups", "TeacherId", "dbo.Accounts");
            DropForeignKey("dbo.AnswerSheets", "TestTakerId", "dbo.Accounts");
            DropForeignKey("dbo.Answers", "AnswerSheetId", "dbo.AnswerSheets");
            DropForeignKey("dbo.Tests", "Question_QuestionId", "dbo.Questions");
            DropForeignKey("dbo.TestDetails", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.TestDetails", "TestId", "dbo.Tests");
            DropForeignKey("dbo.Tests", "TeacherId", "dbo.Accounts");
            DropForeignKey("dbo.Tests", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Courses", "TeacherId", "dbo.Accounts");
            DropForeignKey("dbo.CourseStudent", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.CourseStudent", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.AnswerSheets", "TestId", "dbo.Tests");
            DropForeignKey("dbo.Questions", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "ParentGroupId", "dbo.Groups");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropIndex("dbo.CourseStudent", new[] { "AccountId" });
            DropIndex("dbo.CourseStudent", new[] { "CourseId" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.UserInRoles", new[] { "RoleId" });
            DropIndex("dbo.UserInRoles", new[] { "UserId" });
            DropIndex("dbo.UserProfiles", new[] { "ClassId" });
            DropIndex("dbo.UserProfiles", new[] { "AccountId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Courses", new[] { "TeacherId" });
            DropIndex("dbo.Tests", new[] { "Question_QuestionId" });
            DropIndex("dbo.Tests", new[] { "TeacherId" });
            DropIndex("dbo.Tests", new[] { "CourseId" });
            DropIndex("dbo.TestDetails", new[] { "QuestionId" });
            DropIndex("dbo.TestDetails", new[] { "TestId" });
            DropIndex("dbo.Groups", new[] { "TeacherId" });
            DropIndex("dbo.Groups", new[] { "ParentGroupId" });
            DropIndex("dbo.Questions", new[] { "GroupId" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropIndex("dbo.Answers", new[] { "AnswerSheetId" });
            DropIndex("dbo.AnswerSheets", new[] { "TestTakerId" });
            DropIndex("dbo.AnswerSheets", new[] { "TestId" });
            DropIndex("dbo.Accounts", "UserNameIndex");
            DropTable("dbo.CourseStudent");
            DropTable("dbo.Roles");
            DropTable("dbo.UserInRoles");
            DropTable("dbo.Classes");
            DropTable("dbo.UserProfiles");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Courses");
            DropTable("dbo.Tests");
            DropTable("dbo.TestDetails");
            DropTable("dbo.Groups");
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
            DropTable("dbo.AnswerSheets");
            DropTable("dbo.Accounts");
        }
    }
}
