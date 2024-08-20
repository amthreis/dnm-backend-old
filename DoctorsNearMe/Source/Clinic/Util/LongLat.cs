using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

public class LongLat
{
    [Required]
    [DefaultValue(-43.43522631274805)]
    public double Long { get; set; }

    [Required]
    [DefaultValue(-22.75026077451582)]
    public double Lat { get; set; }

    static GeometryFactory gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);
    
    public LongLat(double lng, double lat)
    {
        Long = lng;
        Lat = lat;
    }

    public LongLat()
    {
        
    }

    public Point ToPoint()
    {
        return gf.CreatePoint(new Coordinate(Long, Lat));
    }
}