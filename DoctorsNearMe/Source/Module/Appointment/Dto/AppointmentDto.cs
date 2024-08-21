using System.Text.Json.Serialization;

namespace DoctorsNearMe;

public class AppointmentDto
{
    public int Id { get; set; }

    public PatientDto Patient { get; set; }
    public DoctorDto Doctor { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AppointmentState State { get; set; } = AppointmentState.BeforeConfirmation;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ReviewScore ReviewScore { get; set; }

    public string ReviewContent { get; set; }
}