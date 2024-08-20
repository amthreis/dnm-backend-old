using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;

public class Clinic
{
    public int Id { get; set;}

    public string Name { get; set; }

    public Point Location { get; set; }

    public double GetMetersTo(Point p)
    {
        var cL = new Point(Location.X, Location.Y) { SRID = 4326 };
        var fL = new Point(p.X, p.Y) { SRID = 4326 };

        return CalculateDistance(cL, fL);
    }

    double CalculateDistance(Point point1, Point point2)
    {
        var d1 = point1.Y * (Math.PI / 180.0);
        var num1 = point1.X * (Math.PI / 180.0);
        var d2 = point2.Y * (Math.PI / 180.0);
        var num2 = point2.X * (Math.PI / 180.0) - num1;
        var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                 Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
        return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
    }

}