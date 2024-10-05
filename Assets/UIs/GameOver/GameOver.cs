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
        ui = GetComponent<VisualElement>();
        restartButton = ui.Q<Button>("restart");
        quitButton = ui.Q<Button>("quit");

        restartButton.clicked += restartRelay.RaiseEvent;
        quitButton.clicked += HanldeQuit;
    }
    private void HanldeQuit() {
        Application.Quit();
    }

}
