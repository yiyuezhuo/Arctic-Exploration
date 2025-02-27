using UnityEngine;

using System;
using System.Collections.Generic;
using ArcticCore;

public class ShipView3: MonoBehaviour, IEarthObject
{
    public Ship model;
    public GameObject icon;
    public GameObject crossSection;

    protected Material material;

    public float GetLongtitudeDeg() => model.longitudeDeg;
    public float GetLatitudeDeg() => model.latitudeDeg;

    public bool hovering;
    public bool selected;
    protected SelectState oldSelectState;

    public enum SelectState
    {
        None = 0,
        Hovering = 1,
        Selected = 2
    }

    public void Start()
    {
        var meshRenderer = icon.GetComponent<MeshRenderer>();
        material = meshRenderer.material = meshRenderer.material; // copy material
    }

    public void SyncSelectState(SelectState state)
    {
        var value = (float)(int)state;
        material.SetFloat("_ShowBorder", value);
    }

    public SelectState GetSelectState()
    {
        if(selected)
            return SelectState.Selected;
        else if(hovering)
            return SelectState.Hovering;
        return SelectState.None;
    }

    public void Update()
    {
        // Sync select state (material color)
        var selectState = GetSelectState();
        if(selectState != oldSelectState)
        {
            SyncSelectState(selectState);
            oldSelectState = selectState;
        }

        transform.localEulerAngles = new Vector3(model.latitudeDeg, -model.longitudeDeg, 0);

        // sync arrow direction
        // var headingArrowR = 0.8f;
        // var zDeg = Utils.TrueNorthClockwiseDegToUnityDeg(model.headingDeg);
        // directionalRoot.transform.localEulerAngles = new Vector3(0, 0, zDeg);
        
        // var zRag = zDeg * Mathf.Deg2Rad;
        // var xOffset = Mathf.Cos(zRag) * headingArrowR;
        // var yOffset = Mathf.Sin(zRag) * headingArrowR;
        // directionalRoot.transform.localPosition = new Vector3(xOffset, yOffset, 0);

        crossSection.transform.localEulerAngles = new Vector3(0, 0, -model.headingDeg);

        // TODO: Handle left click navigation here?
    }
}