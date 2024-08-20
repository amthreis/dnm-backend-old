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

            var lm = new Patient
            {
                User = new User
                {
                    Email = "leila@m.com",
                    Name = "Leila"
                }
            };
            
            ctx.Patient.Add(lm);

            var a = new Patient
            {
                User = new User
                {
                    Email = "andreia@m.com",
                    Name = "Andreia"
                }
            };
            
            ctx.Patient.Add(a);

            var ms = new Patient
            {
                User = new User
                {
                    Email = "marcos@m.com",
                    Name = "Marcos"
                }
            };
            
            ctx.Patient.Add(ms);

            var drAJ = new Doctor
            {
                User = new User
                {
                    Email = "alberto.dr@m.com",
                    Name = "Dr. Alberto"
                }
            };
            
            ctx.Doctor.Add(drAJ);

            ctx.Clinic.Add(new Clinic
            {
                Name = "E.M.",
                Location = gf.CreatePoint(new NetTopologySuite.Geometries.Coordinate(-43.38826359022361, -22.78898601461167))
            });

            var sesc = new Clinic
            {
                Name = "SESC",
                Location = gf.CreatePoint(new NetTopologySuite.Geometries.Coordinate(-43.449800391530275, -22.747648724721763))
            };
            ctx.Clinic.Add(sesc);

            ctx.Clinic.Add(new Clinic
            {
                Name = "Fabinho Parabrisas",
                Location = gf.CreatePoint(new NetTopologySuite.Geometries.Coordinate(-43.43675740742485, -22.751377068032472))
            });

            ctx.Appointment.Add(new Appointment
            {
                ReviewedAt = DateTime.Now,
                Clinic = sesc,
                Doctor = drAJ,
                Patient = lm,
                ReviewScore = ReviewScore.Neutral,
                ReviewContent = "okish treatment",
                CreatedAt = DateTime.Now
            });

            
            ctx.Appointment.Add(new Appointment
            {
                ReviewedAt = DateTime.Now,
                Clinic = sesc,
                Doctor = drAJ,
                Patient = ms,
                ReviewScore = ReviewScore.Negative,
                ReviewContent = "doc treated me like I was stupid or something smh",
                CreatedAt = DateTime.Now
            });

            ctx.Appointment.Add(new Appointment
            {
                ReviewedAt = DateTime.Now,
                Clinic = sesc,
                Doctor = drAJ,
                Patient = a,
                ReviewScore = ReviewScore.Neutral,
                CreatedAt = DateTime.Now
            });


            ctx.SaveChanges();
        }
    }
}