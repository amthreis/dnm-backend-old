using System.Text.Json.Serialization;
using DoctorsNearMe;

public class ReviewAppointmentDto
{
    public int PatientUserId { get; set; }
    public int AppointmentId { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ReviewScore ReviewScore { get; set; }

    public string ReviewContent { get; set; }
}