using UnityEngine;
using UnityEngine.UIElements;

public partial class MainScreen : MonoBehaviour
{
    public VisualElement ui;

    [SerializeField] private VoidChannel playRelay;
    [SerializeField] private VoidChannel continueRelay;

    public Button playButton;
    public Button continueButton;
    public Button quitButton;

    private void Awake() {
        ui = GetComponent<UIDocument>().rootVisualElement;
    }
    private void OnEnable() {
        playButton = ui.Q<Button>("playButton");
        playButton.clicked += OnPlayButtonClicked;

        continueButton = ui.Q<Button>("continueButton");
        continueButton.clicked += OnContinueButtonClicked;

        quitButton = ui.Q<Button>("quitButton");
        quitButton.clicked += OnQuitButtonClicked;


    }
    private void OnPlayButtonClicked() {
        gameObject.SetActive(false);
        playRelay.RaiseEvent();
    }
    private void OnContinueButtonClicked() {
        gameObject.SetActive(false);
        continueRelay.RaiseEvent();
    }
    private void OnQuitButtonClicked() { 
        //Application.Quit();
    }
}
