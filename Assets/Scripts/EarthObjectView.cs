using UnityEngine;
using System;
using System.Collections.Generic;
using ArcticCore;

public interface IEarthObject
{
    public float GetLongtitudeDeg();
    public float GetLatitudeDeg();
}


public class EarthObjectView: MonoBehaviour
{
    protected IEarthObject model;

    public float scaleFactor = 1;

    public void Start()
    {
        model = GetComponent<IEarthObject>();
        if(model == null)
        {
            Debug.LogWarning($"No valid model is specified for {gameObject}");
        }
    }

    static float r = 0.5f;

    public void Update()
    {
        if(model == null)
            return;

        // var cam = CameraController.Instance.camIcon;
        var cam = CameraController2.Instance.camIcon;

        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                         cam.transform.rotation * Vector3.up);

        transform.localScale = Vector3.one * cam.orthographicSize * scaleFactor;

        transform.localPosition = Utils.LatitudeLongitudeDegToVector3(model.GetLatitudeDeg(), model.GetLongtitudeDeg(), r);

        // var latRad = model.GetLatitudeDeg() * Mathf.Deg2Rad;
        // var lonRad = model.GetLongtitudeDeg() * Mathf.Deg2Rad;

        // var r = 0.5f;

        // var y = r * Mathf.Sin(latRad);
        // var hr = Mathf.Abs(r * Mathf.Cos(latRad));
        // var x = hr * Mathf.Sin(lonRad);
        // var z = hr * -Mathf.Cos(lonRad);

        // transform.localPosition = new Vector3(x, y, z);
    }
}