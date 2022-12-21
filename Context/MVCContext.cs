using Microsoft.EntityFrameworkCore;
using LoginPageViaRepositoryPattern.Models;

namespace LoginPageViaRepositoryPattern.Context
{
    public class MVCContext: DbContext
    {
        public MVCContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<Users> uservar { get; set; }
    }
}
