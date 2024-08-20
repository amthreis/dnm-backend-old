using System.Text.Json.Serialization;

namespace DoctorsNearMe;

public class AppointmentDto
{
    public int Id { get; set; }

    public PatientDto Patient { get; set; }
    public DoctorDto Doctor { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ReviewScore ReviewScore { get; set; }
    public string ReviewContent { get; set; }
}