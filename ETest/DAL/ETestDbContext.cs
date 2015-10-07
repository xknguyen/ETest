using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using ETest.Models;

namespace ETest.DAL
{
    public class ETestDbContext : IdentityDbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AnswerSheet> AnswerSheets { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionDetail> QuestionDetails { get; set; }
        public DbSet<TestDetail> QuestionTests { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Thiết lập tên cho các bảng lưu trữ thông tin người dùng và quyền
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<IdentityUser>().ToTable("Accounts");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserInRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");

            // Thiết lập quan hệ giữa nhóm câu hỏi và câu hỏi
            modelBuilder.Entity<Group>()
                .HasMany(o => o.Questions)
                .WithRequired(d => d.Group)
                .HasForeignKey(d => d.GroupId)
                .WillCascadeOnDelete(true);

            // Thiết lập quan hệ giữa tài khoản học sinh và phiếu trả lời
            modelBuilder.Entity<Account>()
                .HasMany(o => o.AnswerSheets)
                .WithRequired(d => d.TestTaker)
                .HasForeignKey(d => d.TestTakerId)
                .WillCascadeOnDelete(true);

            // Thiết lập quan hệ giữa khóa học và Sinh viên
            modelBuilder.Entity<Course>()
                .HasMany(o => o.Students)
                .WithMany(d => d.Courses)
                .Map(m => m.MapLeftKey("CourseId").MapRightKey("AccountId").ToTable("CourseStudent"));

            // Thiết lập quan hệ giữa tài khoản giáo viên và khóa học
            modelBuilder.Entity<Course>().HasRequired(s => s.Teacher).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<Account>()
            //    .HasMany(o => o.Courses)
            //    .WithRequired(d => d.Teacher)
            //    .HasForeignKey(d => d.TeacherId)
            //    .WillCascadeOnDelete(true);

            // Thiết lập quan hệ giữa tài khoản giáo viên với nhóm câu hỏi
            modelBuilder.Entity<Account>()
                .HasMany(o => o.Groups)
                .WithRequired(d => d.Teacher)
                .HasForeignKey(d => d.TeacherId)
                .WillCascadeOnDelete(false);

            // Thiết lập mối quan hệ giữa nhóm con và nhóm cha
            modelBuilder.Entity<Group>()
                .HasMany(c => c.ChildGroups)
                .WithOptional(c => c.ParentGroup)
                .HasForeignKey(c => c.ParentGroupId)
                .WillCascadeOnDelete(false);

            // Thiết lập quan hệ giữa câu hỏi với câu trả lời
            modelBuilder.Entity<Question>()
                .HasMany(o => o.Answers)
                .WithRequired(d => d.Question)
                .HasForeignKey(d => d.QuestionId)
                .WillCascadeOnDelete(true);

            // Thiết lập quan hệ giữa câu hỏi với chi tiết bài kiểm tra
            modelBuilder.Entity<Question>()
                .HasMany(o => o.TestDetails)
                .WithRequired(d => d.Question)
                .HasForeignKey(d => d.QuestionId)
                .WillCascadeOnDelete(true);

            // Thiết lập quan hệ giữa bài kiểm tra với chi tiết bài kiểm tra
            modelBuilder.Entity<Test>()
                .HasMany(o => o.TestDetails)
                .WithRequired(d => d.Test)
                .HasForeignKey(d => d.TestId)
                .WillCascadeOnDelete(true);

            // Thiết lập quan hệ giữa bài kiểm tra với khóa học
            modelBuilder.Entity<Course>()
                .HasMany(o => o.Tests)
                .WithRequired(d => d.Course)
                .HasForeignKey(d => d.CourseId)
                .WillCascadeOnDelete(true);

            // Thiết lập quan hệ giữa bài kiểm tra với phiếu trả lời
            modelBuilder.Entity<Test>()
                .HasMany(o => o.AnswerSheets)
                .WithRequired(d => d.Test)
                .HasForeignKey(d => d.TestId)
                .WillCascadeOnDelete(true);

            // Thiết lập quan hê giữa câu trả lời với phiếu trả lời
            modelBuilder.Entity<AnswerSheet>()
                .HasMany(o => o.Answers)
                .WithRequired(d => d.AnswerSheet)
                .HasForeignKey(d => d.AnswerSheetId)
                .WillCascadeOnDelete(true);

            // Thiết lập quan hệ giữa câu hỏi với chi tiết câu hỏi
            modelBuilder.Entity<Question>()
                .HasMany(q=>q.QuestionDetails)
                .WithRequired(q=>q.Question)
                .HasForeignKey(d=>d.QuestionId)
                .WillCascadeOnDelete(true);
        }

        public ETestDbContext()
            : base("DefaultConnection")
        {
        }

        public static ETestDbContext Create()
        {
            return new ETestDbContext();
        }

    }
}