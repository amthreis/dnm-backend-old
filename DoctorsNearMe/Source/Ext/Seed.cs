using System.Diagnostics;

namespace DoctorsNearMe;

public static class Seeder
{
    public static void UseDbSeeder(this IApplicationBuilder app)
    {
        var factory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
        using var serviceScope = factory.CreateScope();
        var ctx = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

        if(ctx.Database.EnsureCreated()) // false when db already exists
        {
            var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

            Debug.WriteLine("Seeding...");

            ctx.Clinic.Add(new Clinic
            {
                Name = "E.M.",
                Location = gf.CreatePoint(new NetTopologySuite.Geometries.Coordinate(-43.38826359022361, -22.78898601461167))
            });

            ctx.Clinic.Add(new Clinic
            {
                Name = "SESC",
                Location = gf.CreatePoint(new NetTopologySuite.Geometries.Coordinate(-43.449800391530275, -22.747648724721763))
            });

            ctx.Clinic.Add(new Clinic
            {
                Name = "Fabinho Parabrisas",
                Location = gf.CreatePoint(new NetTopologySuite.Geometries.Coordinate(-43.43675740742485, -22.751377068032472))
            });

            ctx.SaveChanges();
        }
    }
}