using Microsoft.EntityFrameworkCore;
using Torvnen.ProjectApi.Model.Entity;

namespace Torvnen.ProjectApi.Data
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().HasOne(p => p.Customer).WithMany(c => c.Projects);
        }

        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Customer> Customers => Set<Customer>();
    }
}