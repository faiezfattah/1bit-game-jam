using UnityEngine;
using UnityEngine.UIElements;

public class Pause : MonoBehaviour {
    private VisualElement ui;
    private UIDocument uiDoc;

    [SerializeField] private InputReader inputReader;
    [SerializeField] private VoidChannel unpauseRelay;
    [SerializeField] private VoidChannel saveRelay;
    [SerializeField] private VoidChannel quitRelay;

    private Button saveButton;
    private Button unpauseButton;
    private Button quitButton;
    private bool isPaused = false;

    private void Awake() {
        uiDoc = GetComponent<UIDocument>();
        uiDoc.enabled = false;
    }

    private void SetupUI() {
        Debug.Log("setup UI called");
        uiDoc = GetComponent<UIDocument>();
        ui = uiDoc.rootVisualElement;

        saveButton = ui.Q<Button>("save");
        unpauseButton = ui.Q<Button>("unpause");
        quitButton = ui.Q<Button>("quit");

        if (saveButton != null) saveButton.clicked += OnSaveButtonClicked;
        if (unpauseButton != null) unpauseButton.clicked += OnContinueButtonClicked;
        if (quitButton != null) quitButton.clicked += OnQuitButtonClicked;
    }

    private void OnEnable() {
        inputReader.EscapeEvent += HandlePause;
    }

    private void HandlePause() {
        isPaused = !isPaused;
        uiDoc.enabled = isPaused;
        if (isPaused) SetupUI();
    }

    private void OnSaveButtonClicked() {
        saveRelay.RaiseEvent();
    }

    private void OnContinueButtonClicked() {
        unpauseRelay.RaiseEvent();
        HandlePause();
    }

    private void OnQuitButtonClicked() {
        quitRelay.RaiseEvent();
    }

    private void OnDisable() {
        if (inputReader != null) {
            inputReader.EscapeEvent -= HandlePause;
        }

        if (saveButton != null) saveButton.clicked -= OnSaveButtonClicked;
        if (unpauseButton != null) unpauseButton.clicked -= OnContinueButtonClicked;
        if (quitButton != null) quitButton.clicked -= OnQuitButtonClicked;
    }
}