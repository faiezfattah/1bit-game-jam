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
    [SerializeField] private VoidChannel playRelay;
    [SerializeField] private VoidChannel loadRelay;
    [SerializeField] private VoidChannel saveRelay;
    [SerializeField] private VoidChannel unpauseRelay;
    [SerializeField] private VoidChannel rebuildRelay;
    [SerializeField] private VoidChannel gameoverRelay;
    [SerializeField] private VoidChannel restartRelay;
    [Header("uis-----------------")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject mainMenuScreen;

    private bool isPaused = false;
    private void Start() {
        Pause();
    }

    private void Update() {
        gameTime.UpdateGameTime();
    }

    private void Pause()
    {
        if (isPaused) Time.timeScale = 1f;
        if (!isPaused) Time.timeScale = 0f;

        isPaused = !isPaused;
    }
    private void HandlePlay() {
        Pause();
        SaveSystem.ResetPlayer(build, economy, gameTime);
    }
    private void HandleSave() {
        Debug.Log("saved player data");
        SaveSystem.SavePlayer(build, economy, gameTime);
    }
    private void HandleLoad() {
        SaveSystem.LoadPlayer(build, economy, gameTime);
        build.GetBuildsDictionary();
        rebuildRelay.RaiseEvent();
        Pause();
    }
    private void HandleGameOver() {
        Pause();
        Instantiate(gameOverScreen);
    }
    private void HandleRestart() {
        SaveSystem.ResetPlayer(build, economy, gameTime);
        SaveSystem.SavePlayer(build, economy, gameTime);
        rebuildRelay.RaiseEvent();
        Pause();
    }
    private void HandleQuit() {
        mainMenuScreen.SetActive(true);
        SaveSystem.SavePlayer(build, economy, gameTime);
    }
    private void OnEnable()
    {
        inputReader.EscapeEvent += Pause;
        unpauseRelay.OnEvenRaised += Pause;
        playRelay.OnEvenRaised += HandlePlay;
        saveRelay.OnEvenRaised += HandleSave;
        loadRelay.OnEvenRaised += HandleLoad;
        gameoverRelay.OnEvenRaised += HandleGameOver;
        restartRelay.OnEvenRaised += HandleRestart;
    }
    private void OnDisable()
    {
        inputReader.EscapeEvent -= Pause;
        unpauseRelay.OnEvenRaised -= Pause;
        playRelay.OnEvenRaised -= HandlePlay;
        saveRelay.OnEvenRaised -= HandleSave;
        loadRelay.OnEvenRaised -= HandleLoad;
        gameoverRelay.OnEvenRaised -= HandleGameOver;
        restartRelay.OnEvenRaised -= HandleRestart;
    }
}

