using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;

public class CreateOrUpdateClinicDto
{
    [Required]
    [DefaultValue("Clinic")]
    public string Name { get; set; }
    
    [Required,Length(2, 2), DefaultValue(new double[2] { -22.75026077451582, -43.43522631274805 })]
    public double[] Coords { get; set; }

    [JsonIgnore]
    public LongLat LongLat => new LongLat(Coords[1], Coords[0]);
}