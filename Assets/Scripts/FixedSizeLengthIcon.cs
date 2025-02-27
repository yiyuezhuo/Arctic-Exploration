using UnityEngine;
using System;

public class FixedSizeLengthIcon : MonoBehaviour
{
    public float scaleFactor = 1;
    public float yScaleFactor = 0.1f;
    public float projectionFactor = 0.1f;
    public bool useProjection = false;
    public Transform lookatTransnform;
    public GameObject root;

    public void Update()
    {

        var cam = CameraController.Instance.camIcon;

        transform.localScale = Vector3.one * cam.orthographicSize * scaleFactor;
        
        if(!useProjection)
        {
            transform.localPosition = new Vector3(0, cam.orthographicSize * yScaleFactor, 0);
        }
        else
        {
            var lookatLocal = lookatTransnform.InverseTransformPoint(transform.position);
            var lookatLocalProjection = new Vector3(lookatLocal.x, lookatLocal.y, 0);
            var s = lookatLocalProjection.magnitude / projectionFactor;
            transform.localPosition = transform.localPosition / Mathf.Max(s, 0.1f);

            lookatLocal = lookatTransnform.InverseTransformPoint(transform.position);
            var projectedAngleRad = Mathf.Atan2(lookatLocal.y, lookatLocal.x);
            var projectedAngleDeg = projectedAngleRad * Mathf.Rad2Deg;

            transform.localEulerAngles = lookatTransnform.localEulerAngles + new Vector3(0, 0, projectedAngleDeg);
        }
    }
}