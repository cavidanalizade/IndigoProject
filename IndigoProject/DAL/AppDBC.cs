using IndigoProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IndigoProject.DAL
{
    public class AppDBC : IdentityDbContext<AppUser>
    {
        public AppDBC(DbContextOptions<AppDBC> options) : base(options)
        {
        }
        public DbSet<Post> posts { get; set; }
    }
}
