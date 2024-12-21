using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [Header("player------------------")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private PlayerGameTime gameTime;
    [SerializeField] private PlayerEconomy economy;
    [SerializeField] private PlayerBuild build;
    
    [Header("NEW relays------------------")] [SerializeField]
    private SoundRelay soundRelay;
    
    [Header("relays------------------")]
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
    
    // TODO: rework these 3 ui to be managed by this
    [SerializeField] private UIDocument pauseScreen;
    [SerializeField] private GameObject tutorial1;
    [SerializeField] private GameObject tutorial2;
    
    [Header("musics")]
    [SerializeField] private AudioClip game;

    private bool isGameover = false;
    private GameObject gameoverUIInstancce;

    private bool isPaused = true;
    private void Start() {
        Pause(false);
    }

    private void Update() {
        gameTime.UpdateGameTime();
    }

    private void Pause(bool value) {
        var soundEvent = new SoundEvent {
            Audioclip = game,
            Soundtype = SoundType.Music,
            Type      = value ? SoundEventType.Pause : SoundEventType.Play
        };
        if (value == false) { 
            Time.timeScale = 1f;
            soundRelay.RaiseEvent(soundEvent);
            isPaused = value;
        }
        if (value == true) { 
            Time.timeScale = 0f;
            isPaused = value;
            soundRelay.RaiseEvent(soundEvent);
        }
        inputReader.mouseClickIntercept = value;
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
        Pause(true);
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
        SaveSystem.SavePlayer(build, economy, gameTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void HandleRestart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnEnable() {
        gameoverRelay.OnEvenRaised += HandleGameOver;
        togglePauseRelay.OnEvenRaised += TogglePause;
        inputReader.EscapeEvent += TogglePause;
        saveRelay.OnEvenRaised += HandleSave;
        quitRelay.OnEvenRaised += HandlePseudoQuit;
        restartRelay.OnEvenRaised += HandleRestart;
    }
    private void OnDisable() {
        gameoverRelay.OnEvenRaised -= HandleGameOver;
        togglePauseRelay.OnEvenRaised -= TogglePause;
        inputReader.EscapeEvent -= TogglePause;
        saveRelay.OnEvenRaised -= HandleRestart;
        quitRelay.OnEvenRaised -= HandlePseudoQuit;
        restartRelay.OnEvenRaised -= HandlePlay;
    }
}

