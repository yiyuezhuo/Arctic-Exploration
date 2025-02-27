using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public Camera cam;
    public Camera camIcon;
    // Vector2 prevMousePos;
    // Vector2 prevCamPos;
    bool dragging = false;

    // public float MovingSpeed = 0.1f;
    public float zoomSpeed = 1f;
    public float zSpeed = 0.25f;

    Vector3 lastTrackedPos;

    public enum ScrollMode
    {
        Orthographic,
        Perspective
    }

    public ScrollMode mode;

    // static Vector2 mouseAdjustedCoef = new Vector2(1, -1);
    static Vector3 mouseAdjustedCoef = new Vector3(1, 1, 1);

    Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        // cam = GetComponent<Camera>();
        initialPosition = transform.position;
    }

    public void ResetToInitialPosition()
    {
        if(initialPosition != Vector3.zero)
            transform.position = initialPosition;
    }

    Vector3 GetHitPoint()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        // var plane = new Plane(Vector3.forward, Vector3.zero);
        if(Physics.Raycast(ray, out var hit, 100))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    void UpdateHitPoint()
    {
        lastTrackedPos = GetHitPoint();
    }

    void DragHitPoint()
    {
        var newTrackedPos = GetHitPoint();
        // Debug.Log(newTrackedPos);
        var diff = newTrackedPos - lastTrackedPos;
        // Debug.Log($"Before: {transform.position}");
        // transform.Translate(-diff * mouseAdjustedCoef);
        // var diff2 = diff * mouseAdjustedCoef;
        // transform.position = transform.position - new Vector3(diff2.x, diff2.y, 0);
        transform.position = transform.position - new Vector3(diff.x * mouseAdjustedCoef.x, 0, diff.z * mouseAdjustedCoef.z);
        // Debug.Log($"After: {transform.position}");
        UpdateHitPoint();
    }

    void UpdateZoom(Camera cam)
    {
        switch(mode)
        {
            case ScrollMode.Orthographic:
                var newSize = cam.orthographicSize - Input.mouseScrollDelta.y * zoomSpeed;
                if (newSize > 0.01f)
                {
                    cam.orthographicSize = newSize;
                    GetHitPoint();
                }
                break;
            case ScrollMode.Perspective:
                var newZ = cam.transform.position.z + Input.mouseScrollDelta.y * zSpeed;
                if (cam.transform.position.z * newZ < 0)
                    break;
                cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, newZ);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        // Zoom
        // if(Input.mouseScrollDelta.y != 0 && EventSystem.current && !EventSystem.current.IsPointerOverGameObject())
        // if(Input.mouseScrollDelta.y != 0 && EventSystem.current && !UnityUtils.IsPointerOverNonIconUI())
        if(Input.mouseScrollDelta.y != 0)
        {
            UpdateZoom(cam);
            UpdateZoom(camIcon);
        }

        // Dragging Navigation
        if(Input.GetMouseButton(1))
        {
            // var mousePosition = (Vector2)Input.mousePosition * mouseAdjustedCoef;
            if (!dragging)
            {
                dragging = true;
                UpdateHitPoint();
            }
            else
            {
                DragHitPoint();
            }
        }
        else
        {
            dragging = false;
        }
    }

    static CameraController _instance;
    public static CameraController Instance
    {
        get
        {
            if(_instance == null)
                _instance = FindFirstObjectByType<CameraController>();
            return _instance;
        }
    }

    public void OnDestroy()
    {        
        if(_instance == this)
            _instance = null;
    }
}
