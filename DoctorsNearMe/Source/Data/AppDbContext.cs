using Microsoft.EntityFrameworkCore;

namespace DoctorsNearMe;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) {}

    public DbSet<Clinic> Clinic { get; set; }
    public DbSet<Appointment> Appointment { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Appointment>()
        //     .Property(p => p.ReviewScore)
        //     .HasConversion<string>(
        //         v => v.ToString(),
        //         v => (Rev)Enum.Parse(typeof(EquineBeast), v)
        //     ); 

        base.OnModelCreating(modelBuilder);
    }
}