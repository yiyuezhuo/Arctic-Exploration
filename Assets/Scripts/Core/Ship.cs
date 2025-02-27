
using System;

namespace ArcticCore
{


[Serializable]
public class Ship
{
    public float longitudeDeg;
    public float latitudeDeg;
    public float shipKnot;
    public float headingDeg; // true north - clockwise (0 deg => North, 90 deg => East)


    public void Move(float hours)
    {
        // Core.LatitudeLongitudeDegToHeightMeter()
        var distNm = hours * shipKnot;
        var distM = distNm * 1854;
        (var newLatitudeDegD, var newLongitudeDegD) = GeoUtils.CalculateNewPosition(latitudeDeg, longitudeDeg, headingDeg, distM);
        var newLatitudeDeg = (float)newLatitudeDegD;
        var newLongitudeDeg = (float)newLongitudeDegD;

        var newHeight = Core.LatitudeLongitudeDegToHeightMeter(newLatitudeDeg, newLongitudeDeg);
        if(newHeight > 0) // block movement
            return;

        longitudeDeg = newLongitudeDeg;
        latitudeDeg = newLatitudeDeg;
    }
}

}