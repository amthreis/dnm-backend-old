namespace DoctorsNearMe;

public class DoctorWithAppointmentsDto
{
    public User User { get; set; }

    public List<AppointmentOfDoctorDto> Appointments { get; set; }
}