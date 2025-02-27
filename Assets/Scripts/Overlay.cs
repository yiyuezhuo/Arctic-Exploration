using UnityEngine;
using UnityEngine.UIElements;

public class Overlay : MonoBehaviour
{
    public Transform controlledCameraTransform;

    VisualElement root;

    Slider cameraRotationSlider;

    public float initialEulerY;

    void Awake()
    {
        var doc = GetComponent<UIDocument>();
        root = doc.rootVisualElement;
        cameraRotationSlider = root.Q<Slider>("CameraRotationSlider");

        initialEulerY = controlledCameraTransform.localEulerAngles.y;

        cameraRotationSlider.RegisterValueChangedCallback(evt => {
            controlledCameraTransform.localEulerAngles = new Vector3(
                controlledCameraTransform.localEulerAngles.x,
                initialEulerY + evt.newValue,
                controlledCameraTransform.localEulerAngles.z
            );
        });
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        root.dataSource = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
