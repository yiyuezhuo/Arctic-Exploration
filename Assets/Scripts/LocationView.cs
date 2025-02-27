
using UnityEngine;
using SharpKml.Engine;
using SharpKml.Dom;


public class LocationView: MonoBehaviour, IEarthObject
{
    public Placemark placemark;

    public float GetLatitudeDeg()
    {
        var point = placemark?.Geometry as Point;
        if(point == null)
            return 0;
        return (float)point.Coordinate.Latitude;
    }
    public float GetLongtitudeDeg()
    {
        var point = placemark?.Geometry as Point;
        if(point == null)
            return 0;
        return (float)point.Coordinate.Longitude;
    }
}