using UnityEngine;
using System;
using SharpKml.Dom;
using System.Linq;

public class LineStringView : MonoBehaviour
{
    public Placemark placemark;

    LineRenderer lineRenderer;

    public void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        var lineString = placemark?.Geometry as LineString;
        if(lineString != null)
        {
            var coords = lineString.Coordinates;
            lineRenderer.positionCount = coords.Count;
            var vector3s = coords.Select(coord => Utils.LatitudeLongitudeDegToVector3((float)coord.Latitude, (float)coord.Longitude, Utils.r));
            lineRenderer.SetPositions(vector3s.ToArray());
        }
    }

    public void Update()
    {
        // if(placemark == null)
        //     return;
    }
}