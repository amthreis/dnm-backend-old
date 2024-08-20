using ProjNet.CoordinateSystems.Projections;
using NetTopologySuite.Geometries;
using System.Diagnostics;

public static class ClinicMapper
{
    
    public static ClinicNearMeDto ToClinicNearMeDto(this Clinic c, Point from, ILogger logger)
    {
        // var d = new Coordinate(c.Location.X, c.Location.Y).Distance(new Coordinate(from.X, from.Y));

        // var cL = new Point(c.Location.X, c.Location.Y) { SRID = 4326 };
        // var fL = new Point(from.X, from.Y) { SRID = 4326 };

        // var distanceInMeters = cL.ProjectTo(2855).Distance(fL.ProjectTo(2855));

        //logger.LogCritical($"{c.Name} ---> c = ({c.Location.X}, {c.Location.Y}) vs from = ({from.X}, {from.Y})");

        return new ClinicNearMeDto
        {
            Id = c.Id,
            Name = c.Name,
            Distance = c.GetMetersTo(from) / 1000
        };
    }

    public static List<ClinicNearMeDto> ToClinicsNearMeDto(this List<Clinic> clinics, Point from, ILogger logger)
    {
        return clinics.Select(c => c.ToClinicNearMeDto(from, logger)).ToList();
    }
}