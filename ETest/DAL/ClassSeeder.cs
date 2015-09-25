using System.Data.Entity.Migrations;
using ETest.Models;

namespace ETest.DAL
{
    public class ClassSeeder
    {
        public static void Seed(ETestDbContext context)
        {

            context.Classes.AddOrUpdate(c => c.ClassName, new Class()
            {
                ClassName = "CTK33"
            }, new Class
            {
                ClassName = "CTK34"
            }, new Class
            {
                ClassName = "CTK35"
            }, new Class
            {
                ClassName = "CTK36"
            });
            context.SaveChanges();
        }
    }
}