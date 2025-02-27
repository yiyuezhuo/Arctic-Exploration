using UnityEngine;
using System;
using System.Collections.Generic;


public class EarthObjectView2: MonoBehaviour
{
    protected IEarthObject model;


    public void Start()
    {
        model = GetComponent<IEarthObject>();
        if(model == null)
        {
            Debug.LogWarning($"No valid model is specified for {gameObject}");
        }
    }

    public void Update()
    {
        if(model == null)
            return;

        transform.localPosition = Utils.LatitudeLongitudeDegToVector3(model.GetLatitudeDeg(), model.GetLongtitudeDeg(), Utils.r);
    }
}