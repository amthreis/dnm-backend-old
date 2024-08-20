using ProjNet.CoordinateSystems.Projections;
using NetTopologySuite.Geometries;

namespace DoctorsNearMe;

public static class AppointmentMapper
{
    public static AppointmentDto ToAppointmentDto(this Appointment appt)
    {
        return new AppointmentDto
        {
            Id = appt.Id,
            Patient = appt.Patient?.ToPatientDto() ?? null,
            Doctor = appt.Doctor?.ToDoctorDto() ?? null
        };
    }

    public static AppointmentOfDoctorDto ToAppointmentOfDoctorDto(this Appointment appt)
    {
        return new AppointmentOfDoctorDto
        {
            Id = appt.Id,
            Patient = appt.Patient.ToPatientDto(),
            ReviewContent = appt.ReviewContent,
            ReviewScore = appt.ReviewScore
        };
    }

    public static List<AppointmentOfDoctorDto> ToAppointmentsOfDoctorDto(this List<Appointment> appts)
    {
        return appts.Select(a => a.ToAppointmentOfDoctorDto()).ToList();
    }
}