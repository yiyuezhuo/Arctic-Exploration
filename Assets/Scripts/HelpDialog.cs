using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class HelpDialog : MonoBehaviour
{
    VisualElement root;

    void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        var confirmButton = root.Q<Button>("ConfirmButton");
        confirmButton.clicked += () => root.style.display = DisplayStyle.None;

        root.style.display = DisplayStyle.None;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!EventSystem.current.IsPointerOverGameObject() && Input.GetKeyDown(KeyCode.H))
        {
            Show();
        }   
    }

    public void Show()
    {
        root.style.display = DisplayStyle.Flex;
    }

    static HelpDialog _Instance;
    public static HelpDialog Instance
    {
        get
        {
            if(_Instance == null)
            {
                _Instance = FindFirstObjectByType<HelpDialog>();
            }
            return _Instance;
        }
    }

    public void OnDestroy()
    {
        if(_Instance == this)
        {
            _Instance = null;
        }
    }
}
