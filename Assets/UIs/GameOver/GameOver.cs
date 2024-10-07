using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOver : MonoBehaviour
{
    [SerializeField] private VoidChannel restartRelay;

    private VisualElement ui;
    private Button restartButton;
    private Button quitButton;

    private void OnEnable() {
        ui = GetComponent<UIDocument>().rootVisualElement;
        restartButton = ui.Q<Button>("restart"); 
        quitButton = ui.Q<Button>("quit");

        restartButton.clicked += HandleRestart;
        quitButton.clicked += HanldeQuit;
    }
    private void HandleRestart() {
        restartRelay.RaiseEvent();
        Destroy(gameObject);
    }
    private void HanldeQuit() {
        Application.Quit();
    }

}
