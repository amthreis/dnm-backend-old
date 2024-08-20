using System.ComponentModel.DataAnnotations.Schema;
using DoctorsNearMe;

public class Patient 
{
    [ForeignKey("Id")]
    public User User { get; set; }

    public List<Appointment> Appointments { get; set; } = new List<Appointment>();
}