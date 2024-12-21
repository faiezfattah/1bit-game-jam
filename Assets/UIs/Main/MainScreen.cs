using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public partial class MainScreen : MonoBehaviour
{
    public VisualElement ui;
    public Button playButton;
    public Button continueButton;
    public Button quitButton;
    private void OnEnable() {
        Setup();
    }
    private void Setup() {
        ui = GetComponent<UIDocument>().rootVisualElement;
        playButton = ui.Q<Button>("playButton");
        playButton.clicked += OnPlayButtonClicked;

        continueButton = ui.Q<Button>("continueButton");
        continueButton.clicked += OnContinueButtonClicked;

        quitButton = ui.Q<Button>("quitButton");
        quitButton.clicked += OnQuitButtonClicked;
    }
    private void OnPlayButtonClicked() {
        gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }
    private void OnContinueButtonClicked() {
        gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }
    private void OnQuitButtonClicked() { 
        Application.Quit();
    }
}
