using System.Text.Json.Serialization;
using DoctorsNearMe;

public class EndAppointmentDto
{
    public int DoctorUserId { get; set; }
    public int AppointmentId { get; set; }
}