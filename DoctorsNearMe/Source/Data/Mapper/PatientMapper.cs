using ProjNet.CoordinateSystems.Projections;
using NetTopologySuite.Geometries;

namespace DoctorsNearMe;

public static class PatientMapper
{
    public static PatientDto ToPatientDto(this Patient patient)
    {
        return new PatientDto
        {
            User = patient.User
        };
    }
}