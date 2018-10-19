using IRunesWebApp.Models;
using Microsoft.EntityFrameworkCore;
using SIS.Demo.Models;

namespace IRunesWebApp.Data
{
    public class IRunesDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder
                .UseSqlServer(@"Server=THINKPAD\SQLEXPRESS;Database=IRunes;Integrated Security=true")
                .UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }
    }
}
