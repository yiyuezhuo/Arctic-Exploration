using System.IO;
using System.IO.Compression;
using ArcticCore;
using UnityEngine;
using System.Linq;
using SharpKml.Engine;
using SharpKml.Dom;
// using Unity.VisualScripting;
using UnityEngine.PlayerLoop;
using UnityEngine.EventSystems;


public class GameManager : MonoBehaviour
{
    public UnityEngine.Camera cam;
    public Texture2D heightTexture;
    public TextAsset kmlFile;

    public GameObject locationPrefab;
    public Transform locationsTransform;
    public Transform shipViewsTransform;

    public GameObject lineStringViewPrefab;
    public Transform lineStringsTransform;
    public LayerMask iconLayerMask;

    // Unity.Collections.NativeArray<short> heightTextureRawArray;
    Unity.Collections.NativeArray<ushort> heightTextureRawArray;

    public float currentLatitude;
    public float currentLongitude;
    public float currentHeight;

    public bool enableLocations;
    public bool enableLineStrings;

    public ShipView3 hoveringShipView;
    public ShipView3 selectedShipView;

    public enum State
    {
        Idle,
        SelectingHeading
    }

    public State state = State.Idle;

    void Awake()
    {
        Core.LatitudeLongitudeDegToHeightMeter = (latDeg, lonDeg) => GetHeight(latDeg, lonDeg, out var _latIdx, out var _lonIdx);
        Core.LatitudeLongitudeDegToIsIcepack = (latDeg, lonDeg) => true; // TODO: Track this state in a specialized texture? But icepack model itself should be defined in the pure core.
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // heightTextureRawArray = heightTexture.GetRawTextureData<short>();
        heightTextureRawArray = heightTexture.GetRawTextureData<ushort>();
        
        Debug.Log($"heightTexture.width={heightTexture.width}, heightTexture.height={heightTexture.height}, heightTextureRawArray.length={heightTextureRawArray.Length}");
        TestHeight(32, 90, 4500, "T");

        // Kml data initialization
        using(var reader = new StringReader(kmlFile.text))
        {
            KmlFile file = KmlFile.Load(reader);
            var kml = file.Root as Kml;
            if(file != null)
            {
                foreach(var placemark in kml.Flatten().OfType<Placemark>())
                {
                    Debug.Log(placemark.Name);

                    var point = placemark.Geometry as Point;
                    if(point != null)
                    {
                        var lon = point.Coordinate.Longitude;
                        var lat = point.Coordinate.Latitude;
                        Debug.Log($"Point: lon={lon}, lat={lat}");

                        var location = Instantiate(locationPrefab, locationsTransform);
                        var locationView = location.GetComponent<LocationView>();
                        locationView.placemark = placemark;
                        var text = location.GetComponentInChildren<TMPro.TMP_Text>();
                        text.text = placemark.Name;
                    }

                    var lineString = placemark.Geometry as LineString;
                    if(lineString != null)
                    {
                        var s = string.Join(",", lineString.Coordinates.Select(c => $"(Lat={c.Latitude}, Lon={c.Longitude})"));
                        Debug.Log(s);

                        var lineStringObject = Instantiate(lineStringViewPrefab, lineStringsTransform);
                        var lineStringView = lineStringObject.GetComponent<LineStringView>();
                        lineStringView.placemark = placemark;
                    }
                }
            }
        }

        Debug.Log(GeoUtils.Test2());
    }

    public void TestHeight(float latDeg, float lonDeg, float expected, string note)
    {
        var h = GetHeight(latDeg, lonDeg, out var latIdx, out var lonIdx);
        Debug.Log($"Lat: {latDeg}, Lon: {lonDeg} => {h}, expected: {expected}, {note} ({latIdx}, {lonIdx})");

        var h2 = GetHeight2(latDeg, lonDeg, out var u, out var v);
        Debug.Log($"Lat: {latDeg}, Lon: {lonDeg} => {h2}, expected: {expected}, {note} ({u}, {v})");

        var h3 = GetHeight(latDeg, lonDeg, out latIdx, out lonIdx);
        Debug.Log($"Lat: {latDeg}, Lon: {lonDeg} => {h3}, expected: {expected}, {note} ({latIdx}, {lonIdx})");
    }

    public ushort GetHeight(float latDeg, float lonDeg, out int latIdx, out int lonIdx)
    {
        lonIdx = (int)Mathf.Floor((lonDeg + 180) / 360 * heightTexture.width);
        latIdx = (int)Mathf.Floor((latDeg + 90) / 180 * heightTexture.height);
        return heightTextureRawArray[latIdx * heightTexture.width + lonIdx];
    }

    public Color GetHeight2(float latDeg, float lonDeg, out float u, out float v)
    {
        u = (lonDeg + 180) / 360 * heightTexture.width;
        v = (latDeg + 90) / 180 * heightTexture.height;
        return heightTexture.GetPixelBilinear(u, v);
    }

    public Color GetHeight3(float latDeg, float lonDeg, out int latIdx, out int lonIdx)
    {
        lonIdx = (int)Mathf.Floor((lonDeg + 180) / 360 * heightTexture.width);
        latIdx = (int)Mathf.Floor((latDeg + 90) / 180 * heightTexture.height);
        return heightTexture.GetPixel(lonIdx, latIdx);
    }

    // Update is called once per frame
    void Update()
    {
        var collied = GetCurrentLatitudeLongitude(out var hitPoint, out var latDeg, out var lonDeg);

        if(collied)
        {
            currentLatitude = latDeg;
            currentLongitude = lonDeg;
            currentHeight = GetHeight(latDeg, lonDeg, out var latIdx, out var lonIdx);

            // if(Input.GetMouseButtonDown(0))
            // {
            //     Debug.Log($"Left Click hit.point={hitPoint}, lat={latDeg}, lon={lonDeg}");
            // }
        }

        locationsTransform.gameObject.SetActive(enableLocations);
        lineStringsTransform.gameObject.SetActive(enableLineStrings);

        if(!EventSystem.current.IsPointerOverGameObject())
        {
            if(state == State.Idle)
            {
                // Unit interaction
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                var shouldClear = true;
                if(Physics.Raycast(ray, out var hit, Mathf.Infinity, iconLayerMask))
                {
                    // var ship = hit.collider.gameObject.GetComponent<ShipView>();
                    // var ship = hit.collider.gameObject.GetComponent<ShipView2>();
                    var ship = hit.collider.gameObject.GetComponent<FixedDirectionalSizeIcon>()?.root?.GetComponent<ShipView3>();
                    if(ship != null)
                    {
                        hoveringShipView = ship;
                        hoveringShipView.hovering = true;

                        shouldClear = false;
                    }
                }
                if(shouldClear)
                {
                    if(hoveringShipView != null)
                    {
                        hoveringShipView.hovering = false;
                        hoveringShipView = null;
                    }
                }

                // Left click select
                if(Input.GetMouseButtonDown(0))
                {
                    if(hoveringShipView != null)
                    {
                        // select
                        selectedShipView = hoveringShipView;
                        selectedShipView.selected = true;
                    }
                    else
                    {
                        // TODO: re-use this as direction selection?
                        // de-select
                        // if(selectedShipView != null)
                        // {
                        //     selectedShipView.selected = false;
                        //     selectedShipView = null;
                        // }
                    }
                }
            }
            else if(state == State.SelectingHeading)
            {
                // 
                if(Input.GetMouseButtonDown(0))
                {
                    if(selectedShipView != null && collied)
                    {
                        var model = selectedShipView.model;
                        var lat1 = model.latitudeDeg;
                        var lon1 = model.longitudeDeg;
                        var lat2 = latDeg;
                        var lon2 = lonDeg;

                        var bearing = (float)GeoUtils.CalculateInitialBearing(lat1, lon1, lat2, lon2);
                        model.headingDeg = bearing;

                        // selectedShipView.model.headingDeg = 
                    }
                    state = State.Idle;
                }
            }

            // Hotkeys
            // if(Input.GetKeyDown(KeyCode.F3))
            if(Input.GetKeyDown(KeyCode.D))
            {
                state = State.SelectingHeading;
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                state = State.Idle;

                // de-select
                if(selectedShipView != null)
                {
                    selectedShipView.selected = false;
                    selectedShipView = null;
                }

            }


        }

    }

    public bool GetCurrentLatitudeLongitude(out Vector3 hitPoint, out float latDeg, out float lonDeg)
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            hitPoint = hit.point;
            // var x = hit.point.x;
            // var y = hit.point.y;
            // var z = hit.point.z;

            // var hr = Mathf.Sqrt(z*z + x*x);
            // var latRad = Mathf.Atan2(y, hr);
            // // var lonRad = Mathf.Acos(-z / hr);
            // var lonRad = Mathf.Atan2(x, -z);

            // latDeg = latRad * Mathf.Rad2Deg;
            // lonDeg = lonRad * Mathf.Rad2Deg;

            (latDeg, lonDeg) = Utils.Vector3ToLatitudeLongitudeDeg(hitPoint);

            return true;

            // Debug.Log($"hit.point={hit.point}, lat={latDeg}, lon={lonDeg}");
        }

        hitPoint = new();
        latDeg = lonDeg = 0;
        return false;
    }

    public void Step(float deltaSeconds)
    {
        foreach(var shipView in shipViewsTransform.GetComponentsInChildren<ShipView3>())
        {
            shipView.Step(deltaSeconds);
        }

        // FogOfWarSphere.Instance?.UpdateFogOfWar(Model.)
    }

    static GameManager _Instance;
    public static GameManager Instance
    {
        get
        {
            if(_Instance == null)
            {
                _Instance = FindFirstObjectByType<GameManager>();
            }
            return _Instance;
        }
    }

    public void OnDestroy()
    {
        Debug.Log("GameManager.OnDestroy");

        if(_Instance == this)
        {
            _Instance = null;
        }
    }
}
