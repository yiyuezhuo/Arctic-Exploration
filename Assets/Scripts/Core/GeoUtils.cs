using System;

public class GeoUtils
{
    private const double EarthRadius = 6371000; // m

    public static (double newLat, double newLon) CalculateNewPosition(double lat, double lon, double bearing, double distance)
    {
        double latRad = DegreesToRadians(lat);
        double lonRad = DegreesToRadians(lon);
        double bearingRad = DegreesToRadians(bearing);

        double newLatRad = Math.Asin(Math.Sin(latRad) * Math.Cos(distance / EarthRadius) +
                              Math.Cos(latRad) * Math.Sin(distance / EarthRadius) * Math.Cos(bearingRad));

        double newLonRad = lonRad + Math.Atan2(Math.Sin(bearingRad) * Math.Sin(distance / EarthRadius) * Math.Cos(latRad),
                                               Math.Cos(distance / EarthRadius) - Math.Sin(latRad) * Math.Sin(newLatRad));

        double newLat = RadiansToDegrees(newLatRad);
        double newLon = RadiansToDegrees(newLonRad);

        return (newLat, newLon);
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    private static double RadiansToDegrees(double radians)
    {
        return radians * 180.0 / Math.PI;
    }

    public static string Test()
    {
        // Lat 40.7128，Lon-74.0060，45 deg（east-west），move 1000m
        var result = CalculateNewPosition(40.7128, -74.0060, 45, 1000);
        return $"New Latitude: {result.newLat}, New Longitude: {result.newLon}";
    }

    public static double CalculateInitialBearing(double lat1, double lon1, double lat2, double lon2)
    {
        // Convert latitude and longitude from degrees to radians
        double lat1Rad = DegreesToRadians(lat1);
        double lon1Rad = DegreesToRadians(lon1);
        double lat2Rad = DegreesToRadians(lat2);
        double lon2Rad = DegreesToRadians(lon2);

        // Calculate the difference in longitude
        double deltaLon = lon2Rad - lon1Rad;

        // Use the spherical trigonometry formula to calculate the initial bearing
        double y = Math.Sin(deltaLon) * Math.Cos(lat2Rad);
        double x = Math.Cos(lat1Rad) * Math.Sin(lat2Rad) - Math.Sin(lat1Rad) * Math.Cos(lat2Rad) * Math.Cos(deltaLon);
        double initialBearingRad = Math.Atan2(y, x);

        // Convert the bearing from radians to degrees
        double initialBearingDeg = RadiansToDegrees(initialBearingRad);

        // Normalize the bearing to the range [0, 360)
        initialBearingDeg = (initialBearingDeg + 360) % 360;

        return initialBearingDeg;
    }

    public static string Test2()
    {
        // Latitude and longitude of the starting point (in degrees)
        double lat1 = 34.0522; // Example: Latitude of Los Angeles
        double lon1 = -118.2437; // Example: Longitude of Los Angeles

        // Latitude and longitude of the destination (in degrees)
        double lat2 = 40.7128; // Example: Latitude of New York
        double lon2 = -74.0060; // Example: Longitude of New York

        // Calculate the initial bearing
        double initialBearing = CalculateInitialBearing(lat1, lon1, lat2, lon2);

        return $"Initial Bearing: {initialBearing} degrees";
    }

    public static double HaversineDistanceKm(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371;

        double dLat = DegreesToRadians(lat2 - lat1);
        double dLon = DegreesToRadians(lon2 - lon1);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        double distance = R * c;

        return distance;
    }
}
