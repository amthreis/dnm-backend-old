using Microsoft.EntityFrameworkCore;

namespace DoctorsNearMe;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) {
        
        ChangeTracker.LazyLoadingEnabled = false;

    }

    public DbSet<Clinic> Clinic { get; set; }
    public DbSet<Appointment> Appointment { get; set; }

    public DbSet<User> User { get; set; }
    public DbSet<Patient> Patient { get; set; }
    public DbSet<Doctor> Doctor { get; set; }

}