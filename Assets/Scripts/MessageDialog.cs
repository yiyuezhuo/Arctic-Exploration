using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MessageDialog : MonoBehaviour
{
    VisualElement root;

    TextField messageTextField;

    Queue<string> messageQueue = new();

    public void Awake()
    {
        var doc = GetComponent<UIDocument>();
        root = doc.rootVisualElement;

        messageTextField = root.Q<TextField>("MessageTextField");
        var confirmButton = root.Q<Button>("ConfirmButton");

        confirmButton.clicked += () => {
            root.style.display = DisplayStyle.None;
        };

        root.style.display = DisplayStyle.None;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(root.style.display == DisplayStyle.None && messageQueue.Count > 0)
        {
            var message = messageQueue.Dequeue();
            messageTextField.SetValueWithoutNotify(message);
            root.style.display = DisplayStyle.Flex;
        }
    }

    public void PopupWith(string message)
    {
        messageTextField.SetValueWithoutNotify(message);

        root.style.display = DisplayStyle.Flex;
    }

    public void QueueMessage(string message)
    {
        messageQueue.Enqueue(message);
    }

    static MessageDialog _Instance;
    public static MessageDialog Instance
    {
        get
        {
            if(_Instance == null)
            {
                _Instance = FindFirstObjectByType<MessageDialog>();
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
