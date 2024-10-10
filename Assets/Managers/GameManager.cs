using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("player------------------")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private PlayerGameTime gameTime;
    [SerializeField] private PlayerEconomy economy;
    [SerializeField] private PlayerBuild build;
    [Header("relays------------------")]
    [SerializeField] private VoidChannel mainMenuPlay;
    [SerializeField] private VoidChannel mainMenuContinue;

    [SerializeField] private AudioChannel musicRelay;
    [SerializeField] private BoolChannel musicStop;

    [SerializeField] private VoidChannel gameoverRelay;
    [SerializeField] private VoidChannel togglePauseRelay;
    [SerializeField] private VoidChannel quitRelay;
    [SerializeField] private VoidChannel restartRelay;

    [SerializeField] private VoidChannel saveRelay;
    [SerializeField] private VoidChannel resetRelay; // reset everything, rebuild everything
    [Header("uis-----------------")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject mainMenuScreen;
    [Header("musics")]
    [SerializeField] private AudioClip game;
    [SerializeField] private AudioClip mainMenu;

    private bool isGameover = false;
    private GameObject gameoverUIInstancce;

    private bool isPaused = true;
    private void Start() {
        if (mainMenu == null) Debug.Log("mainmenu not found");
        musicRelay.RaiseEvent(mainMenu);
        Pause(true);
    }

    private void Update() {
        gameTime.UpdateGameTime();
    }

    private void Pause(bool value)
    {
        if (value == false) { 
            Time.timeScale = 1f;
            musicStop.RaiseEvent(false);
            isPaused = value;
        }
        if (value == true) { 
            Time.timeScale = 0f;
            isPaused = value;
            musicStop.RaiseEvent(true);
        }
    }
    private void TogglePause() {
        Pause(!isPaused);
    }
    private void HandlePlay() {
        if (isGameover) {
            Destroy(gameoverUIInstancce);
            isGameover = false;
        }

        SaveSystem.ResetPlayer(build, economy, gameTime);
        SaveSystem.SavePlayer(build, economy, gameTime);
        resetRelay.RaiseEvent();
        musicRelay.RaiseEvent(game);
        Pause(false);
    }
    private void HandleSave() {
        Debug.Log("saved player data");
        SaveSystem.SavePlayer(build, economy, gameTime);
    }
    private void HandleLoad() {
        SaveSystem.LoadPlayer(build, economy, gameTime);
        resetRelay.RaiseEvent();
        musicRelay.RaiseEvent(game);
        Pause(false);
    }
    private void HandleGameOver() {
        gameoverUIInstancce = Instantiate(gameOverScreen);
        isGameover = true;
        Pause(true);
    }
    private void HandlePseudoQuit() {
        mainMenuScreen.SetActive(true);
        SaveSystem.SavePlayer(build, economy, gameTime);
        Pause(true);
        musicRelay.RaiseEvent(mainMenu);

        if (isGameover) {
            Destroy(gameoverUIInstancce);
            isGameover = false;
        }
    }

    private void OnEnable() {
        mainMenuPlay.OnEvenRaised += HandlePlay;
        mainMenuContinue.OnEvenRaised += HandleLoad;
        gameoverRelay.OnEvenRaised += HandleGameOver;
        togglePauseRelay.OnEvenRaised += TogglePause;
        inputReader.EscapeEvent += TogglePause;
        saveRelay.OnEvenRaised += HandleSave;
        quitRelay.OnEvenRaised += HandlePseudoQuit;
        restartRelay.OnEvenRaised += HandlePlay;
    }
    private void OnDisable() {
        mainMenuPlay.OnEvenRaised -= HandlePlay;
        mainMenuContinue.OnEvenRaised -= HandleLoad;
        gameoverRelay.OnEvenRaised -= HandleGameOver;
        togglePauseRelay.OnEvenRaised -= TogglePause;
        inputReader.EscapeEvent -= TogglePause;
        saveRelay.OnEvenRaised -= HandleSave;
        quitRelay.OnEvenRaised -= HandlePseudoQuit;
        restartRelay.OnEvenRaised -= HandlePlay;
    }
}

