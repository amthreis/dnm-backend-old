using ProjNet.CoordinateSystems.Projections;
using NetTopologySuite.Geometries;

namespace DoctorsNearMe;

public static class DoctorMapper
{
    public static DoctorDto ToDoctorDto(this Doctor doctor)
    {
        return new DoctorDto
        {
            User = doctor.User
        };
    }

    public static DoctorWithAppointmentsDto ToDoctorWithAppointmentsDto(this Doctor doctor)
    {
        return new DoctorWithAppointmentsDto
        {
            User = doctor.User,
            Appointments = doctor.Appointments.ToAppointmentsOfDoctorDto()
        };
    }
}