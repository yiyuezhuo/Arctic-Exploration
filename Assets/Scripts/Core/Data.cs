
using System;
using System.Collections.Generic;

namespace ArcticCore
{

public static class Data
{
    public class Location
    {
        public string name;
        public float latitudeDeg;
        public float longitudeDeg;

        public Location(string name, float latitudeDeg, float longitudeDeg)
        {
            this.name = name;
            this.latitudeDeg = latitudeDeg;
            this.longitudeDeg = longitudeDeg;
        }
    }

    // In favor of kml import
    // public static List<Location> locations = new()
    // {
    //     new("Disko Bay", 69, -52),
    //     new("Greenland", 72, -43),
    //     new("Baffin Bay", 74, -69),
    //     new("Prince of Wales Island", 72.5f, 99),
    //     new("Cornwallis", 75, 94.5f)
    // };
}

}