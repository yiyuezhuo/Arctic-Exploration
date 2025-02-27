using System.Linq;
using SharpKml.Engine;

public static class TestSharpKml
{
    public static void Test(string[] args)
    {
        // This will read a Kml file into memory.
        // KmlFile file = KmlFile.Load("YourKmlFile.kml");

        // Kmz (compressed Kml files) can also be loaded:
        //KmzFile kmz = KmzFile.Open("YourKmzFile.kmz");
        // KmlFile file = kmz.GetDefaultKmlFile();
    }
}