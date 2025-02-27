using UnityEngine;
using System;

public class FixedDirectionalSizeIcon : MonoBehaviour
{
    public float scaleFactor = 1;
    public GameObject root;

    public void Update()
    {

        var cam = CameraController.Instance.camIcon;

        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                         cam.transform.rotation * Vector3.up);

        transform.localScale = Vector3.one * cam.orthographicSize * scaleFactor;
    }
}