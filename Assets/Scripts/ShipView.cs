
using UnityEngine;
using System;
using System.Collections.Generic;
using ArcticCore;

public class ShipView: MonoBehaviour, IEarthObject
{
    public Ship model;

    public float GetLongtitudeDeg() => model.longitudeDeg;
    public float GetLatitudeDeg() => model.latitudeDeg;

    public void Update()
    {
        // TODO: Handle left click navigation here?
    }
}