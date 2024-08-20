using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bogus.DataSets;
using DoctorsNearMe;

public class Doctor 
{
    [ForeignKey("Id")]
    public User User { get; set; }

    public List<Appointment>? Appointments { get; set; }
}