using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.EventSystems;

public class TimeController : MonoBehaviour
{
    private Label timeLabel;
    private Button stepButton;
    private Button startButton;
    private Button pauseButton;
    private SliderInt speedSliderInt; // How many step a real second advanced.
    private SliderInt stepSliderInt; // How many game minutes a step will advance

    public float accRealWorldSeconds;
    public bool playing;

    public DateTime currentDateTime = new DateTime(1848, 8, 1, 10, 0, 0);

    void Awake()
    {
        var uiDocument = GetComponent<UIDocument>();
        var rootVisualElement = uiDocument.rootVisualElement;

        timeLabel = rootVisualElement.Q<Label>("TimeLabel");
        stepButton = rootVisualElement.Q<Button>("StepButton");
        startButton = rootVisualElement.Q<Button>("StartButton");
        pauseButton = rootVisualElement.Q<Button>("PauseButton");
        speedSliderInt = rootVisualElement.Q<SliderInt>("SpeedSliderInt");
        stepSliderInt = rootVisualElement.Q<SliderInt>("StepSliderInt");

        stepButton.clicked += OnStepButtonClicked;
        startButton.clicked += OnStartButtonClicked;
        pauseButton.clicked += OnPauseButtonClicked;

        timeLabel.text = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public void Update()
    {
        if(playing)
        {
            var readlWorldDeltaSeconds = Time.deltaTime;
            
            var realWorldStep = 1f / speedSliderInt.value;

            accRealWorldSeconds += readlWorldDeltaSeconds;
            while(accRealWorldSeconds > realWorldStep)
            {
                accRealWorldSeconds -= realWorldStep;

                Step(stepSliderInt.value * 60);
            }
        }

        Sync();

        // Hotkeys
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                OnStartButtonClicked();
            }
            if(Input.GetKeyDown(KeyCode.Return))
            {
                OnStartButtonClicked();
            }
            if(Input.GetKeyDown(KeyCode.P))
            {
                OnPauseButtonClicked();
            }
        }
    }

    public void Sync() // TODO: Move to data binding.
    {
        var timeValue = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        timeLabel.text = timeValue;
    }

    public void Step(float deltaSeconds)
    {
        GameManager.Instance.Step(deltaSeconds);
        currentDateTime = currentDateTime.AddSeconds(deltaSeconds);
    }

    // void OnDisable()
    // {
    //     stepButton.clicked -= OnStepButtonClicked;
    //     startButton.clicked -= OnStartButtonClicked;
    //     pauseButton.clicked -= OnPauseButtonClicked;

    //     stepSlider.UnregisterValueChangedCallback(OnStepSliderValueChanged);
    //     fidelitySlider.UnregisterValueChangedCallback(OnFidelitySliderValueChanged);
    // }

    private void OnStepButtonClicked()
    {
        Debug.Log("Step button clicked");

        Step(stepSliderInt.value * 60);

        Sync();
    }

    private void OnStartButtonClicked()
    {
        Debug.Log("Start button clicked");

        playing = true;
        accRealWorldSeconds = 0;
    }

    private void OnPauseButtonClicked()
    {
        Debug.Log("Pause button clicked");

        playing = false;
        accRealWorldSeconds = 0;
    }
}
