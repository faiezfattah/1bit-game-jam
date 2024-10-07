using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Sources----")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Relays----")]
    [SerializeField] private AudioChannel sfxRelay;
    [SerializeField] private VoidChannel gameOverRelay;
    [SerializeField] private VoidChannel onGameRelay;
    [SerializeField] private VoidChannel onContinueGameRelay;
    [SerializeField] private VoidChannel stopMusicRelay;

    [Header("Global sounds----")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip gameMusic;
    private void Awake() {
        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

        if (musicSource == null) {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }
        PlayMusic(mainMenuMusic);
    }
    private void PlaySFX(AudioClip clip) {
        sfxSource.PlayOneShot(clip);
    }
    private void PlaySFX(AudioClip clip, Vector2 position) {
        //TODO: make position based audio
    }
    private void PlayMusic(AudioClip clip) {
        musicSource.clip = clip;
        musicSource.Play();
    }
    private void StopMusic() {
        musicSource?.Stop();
    }
    private void PlayGameMusic() {
        PlayMusic(gameMusic);
    }
    private void OnEnable() {
        sfxRelay.OnEventRaised += PlaySFX;
        gameOverRelay.OnEvenRaised += StopMusic; // TODO: change to gameover musics
        onGameRelay.OnEvenRaised += PlayGameMusic;
        onContinueGameRelay.OnEvenRaised += PlayGameMusic;
        stopMusicRelay.OnEvenRaised += StopMusic;
    }    
    private void OnDisable() {
        sfxRelay.OnEventRaised -= PlaySFX;
        gameOverRelay.OnEvenRaised -= StopMusic;
        onGameRelay.OnEvenRaised += PlayGameMusic;
        onContinueGameRelay.OnEvenRaised += PlayGameMusic;
        stopMusicRelay.OnEvenRaised -= StopMusic;
    }
}
