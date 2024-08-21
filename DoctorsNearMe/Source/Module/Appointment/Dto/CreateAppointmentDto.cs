using System.Text.Json.Serialization;

namespace DoctorsNearMe;

public class CreateAppointmentDto
{
    public int PatientUserId { get; set; }
    public int DoctorUserId { get; set; }
    public int ClinicId { get; set; }
}