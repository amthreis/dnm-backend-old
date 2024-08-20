using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DoctorsNearMe;

public enum ReviewScore
{
    Negative,
    Neutral,
    Positive
}

public class Appointment
{
    public int Id { get; set;}

    public Clinic Clinic { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public DateTime? EndedAt { get; set; }

    public DateTime? ReviewedAt { get; set; }

    [Column(TypeName = "TEXT")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ReviewScore ReviewScore { get; set; } = ReviewScore.Neutral;
    
    [Length(2, 450)]
    public string? ReviewContent { get; set; }
}