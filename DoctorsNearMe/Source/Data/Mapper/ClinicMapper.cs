using ProjNet.CoordinateSystems.Projections;
using NetTopologySuite.Geometries;
using System.Diagnostics;

public static class ClinicMapper
{
    public static ClinicNearMeDto ToClinicNearMeDto(this Clinic c, Point from, ILogger logger)
    {
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