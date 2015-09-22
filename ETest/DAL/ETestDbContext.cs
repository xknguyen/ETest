using ETest.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ETest.DAL
{
    public class ETestDbContext : IdentityDbContext
    {
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