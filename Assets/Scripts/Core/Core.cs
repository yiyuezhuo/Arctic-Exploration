using System;

namespace ArcticCore
{

public static class Core
{
    public static Func<float, float, float> LatitudeLongitudeDegToHeightMeter; // (LatitudeDeg, Longitude) => heightM
    public static Func<float, float, bool> LatitudeLongitudeDegToIsIcepack;
}

}