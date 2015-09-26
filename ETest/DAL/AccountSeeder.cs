using System;
using ETest.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ETest.DAL
{
    public class AccountSeeder
    {
        public static void Seed(ETestDbContext context)
        {
            // Tạo các đối tượng quản lý quyền và người dùng
            var userManager = new UserManager<Account>(new UserStore<Account>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            const string adminRole = "Admin",
                teacherRole = "Teacher",
                studentRole = "Student",
                password = "123456";

            // Tạo các quyền (vai trò của người dùng trong hệ thống)
            if (!roleManager.RoleExists(adminRole))
                roleManager.Create(new IdentityRole(adminRole));

            if (!roleManager.RoleExists(teacherRole))
                roleManager.Create(new IdentityRole(teacherRole));

            if (!roleManager.RoleExists(studentRole))
                roleManager.Create(new IdentityRole(studentRole));

            // Tạo tài khoản Admin
            var adminUser = new Account()
            {
                UserName = "khoanv",
                Email = "nguyenxuankhoa2309@gmail.com",
                PhoneNumber = "01687568353",
                Profile = new UserProfile()
                {
                    LastName = "Nguyễn Xuân",
                    FirstName = "Khoa",
                    BirthDate = new DateTime(1994, 8, 12)
                }
            };
            var result = userManager.Create(adminUser, password);
            
            // Gán quyền Admin và Teacher cho người dùng vừa tạo
            if (result.Succeeded)
            {
                userManager.AddToRole(adminUser.Id, adminRole);
                userManager.AddToRole(adminUser.Id, teacherRole);
            }

            var teacherUser = new Account()
            {
                UserName = "thuongnv",
                Email = "thuongnc@gmail.com",
                PhoneNumber = "01687568353",
                Profile = new UserProfile()
                {
                    LastName = "Nguyễn Văn",
                    FirstName = "Thương",
                    BirthDate = new DateTime(1994, 8, 12)
                }
            };

            result = userManager.Create(teacherUser, password);

            // Gán quyền Teacher cho người dùng vừa tạo
            if (result.Succeeded)
            {
                userManager.AddToRole(teacherUser.Id, teacherRole);
            }

            var student = new Account()
            {
                UserName = "huytc",
                Email = "huytc@gmail.com",
                PhoneNumber = "01687568353",
                Profile = new UserProfile()
                {
                    LastName = "Trần Công",
                    FirstName = "Huy",
                    BirthDate = new DateTime(1994, 8, 2)
                }
            };

            result = userManager.Create(student, password);

            // Gán quyền Teacher cho người dùng vừa tạo
            if (result.Succeeded)
            {
                userManager.AddToRole(student.Id, studentRole);
            }

            student = new Account()
            {
                UserName = "tuannm",
                Email = "tuannm@gmail.com",
                PhoneNumber = "01684751874",
                Profile = new UserProfile()
                {
                    LastName = "Nguyễn Minh",
                    FirstName = "Tuấn",
                    BirthDate = new DateTime(1994, 3, 11)
                }
            };

            result = userManager.Create(student, password);

            // Gán quyền Teacher cho người dùng vừa tạo
            if (result.Succeeded)
            {
                userManager.AddToRole(student.Id, studentRole);
            }

        }
        
    }
}