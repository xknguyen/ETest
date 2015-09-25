using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using ETest.Models;

namespace ETest.DAL
{
    public class ETestDbContext : IdentityDbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<AnswerSheet> AnswerSheets { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<TestDetail> QuestionTests { get; set; }
        public DbSet<Test> Tests { get; set; }

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
                .HasForeignKey(d => d.GroupId);

            // Thiết lập quan hệ giữa tài khoản học sinh và lớp
            modelBuilder.Entity<Class>()
                .HasMany(o => o.Students)
                .WithOptional(d => d.Class)
                .HasForeignKey(d => d.ClassId);

            // Thiết lập quan hệ giữa tài khoản học sinh và phiếu trả lời
            modelBuilder.Entity<Account>()
                .HasMany(o => o.AnswerSheets)
                .WithRequired(d => d.TestTaker)
                .HasForeignKey(d => d.TestTakerId);



            // Thiết lập quan hệ giữa khóa học và Sinh viên
            modelBuilder.Entity<Course>()
                .HasMany(o => o.Students)
                .WithMany(d => d.Courses)
                .Map(m => m.MapLeftKey("CourseId").MapRightKey("AccountId").ToTable("CourseStudent"));

            // Thiết lập quan hệ giữa tài khoản giáo viên và khóa học
            //modelBuilder.Entity<Course>().HasRequired(s => s.Teacher);
            //modelBuilder.Entity<Account>()
            //    .HasMany(o => o.Courses)
            //    .WithRequired(d => d.Teacher)
            //    .HasForeignKey(d => d.TeacherId)
            //    .WillCascadeOnDelete(true);

            // Thiết lập quan hệ giữa tài khoản giáo viên với nhóm câu hỏi
            // modelBuilder.Entity<Group>().HasRequired(s => s.Teacher).WithMany(s=>s.Groups).WillCascadeOnDelete(true);
            //modelBuilder.Entity<Account>()
            //    .HasMany(o => o.Groups)
            //    .WithRequired(d => d.Teacher)
            //    .HasForeignKey(d => d.TeacherId);

            // Thiết lập mối quan hệ giữa nhóm con và nhóm cha
            


            // Thiết lập quan hệ giữa câu hỏi với câu trả lời
            

            // Thiết lập quan hệ giữa câu hỏi với chi tiết bài kiểm tra
            

            // Thiết lập quan hệ giữa bài kiểm tra với chi tiết bài kiểm tra
           

            // Thiết lập quan hệ giữa bài kiểm tra với khóa học
            

            // Thiết lập quan hệ giữa bài kiểm tra với phiếu trả lời
            

            // Thiết lập quan hê giữa câu trả lời với phiếu trả lời
            
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