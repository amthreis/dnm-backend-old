using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) {}

    public DbSet<Clinic> Clinic { get; set; }
}