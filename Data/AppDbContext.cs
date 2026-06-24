using Microsoft.EntityFrameworkCore;
using VehicleSubmissionApp.Model;

namespace VehicleSubmissionApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<VehicleModel> Models { get; set; }
        public DbSet<Submission> Submissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleModel>().ToTable("Models");
        }
    }
}