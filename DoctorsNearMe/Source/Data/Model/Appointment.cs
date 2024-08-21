using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace DoctorsNearMe;

public enum ReviewScore
{
    Negative,
    Neutral,
    Positive
}

public enum AppointmentState
{
    BeforeConfirmation,
    AfterConfirmation,
    Ongoing,
    Cancelled,
    Over
}

public class Appointment
{
    public int Id { get; set;}

    public Clinic Clinic { get; set; }
    public Doctor Doctor { get; set; }
    public Patient Patient { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }

    [Column(TypeName = "TEXT")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AppointmentState State { get; set; } = AppointmentState.BeforeConfirmation;

    [Column(TypeName = "TEXT")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ReviewScore ReviewScore { get; set; } = ReviewScore.Neutral;
    
    [Length(2, 450)]
    public string? ReviewContent { get; set; }
    
}