using System.Text.Json.Serialization;
using DoctorsNearMe;

public class AppointmentOfDoctorDto
{
    public int Id { get; set; }

    public PatientDto Patient { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ReviewScore ReviewScore { get; set; }
    public string ReviewContent { get; set; }
}