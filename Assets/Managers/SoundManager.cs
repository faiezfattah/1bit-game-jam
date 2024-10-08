using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Sources----")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Relays----")]
    [SerializeField] private AudioChannel sfxRelay;
    [SerializeField] private AudioChannel musicRelay;
    [SerializeField] private BoolChannel toggleMusicPlay;
    private void Awake() {
        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

        if (musicSource == null) {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }
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
    private void StopMusic(bool value) {
        if (value == true) { 
            musicSource.Pause(); 
        }
        else {
            musicSource.UnPause();
        }
    }
    private void OnEnable() {
        sfxRelay.OnEventRaised += PlaySFX;
        toggleMusicPlay.OnEventRaised += StopMusic;
        musicRelay.OnEventRaised += PlayMusic;
    }    
    private void OnDisable() {
        sfxRelay.OnEventRaised -= PlaySFX;
        toggleMusicPlay.OnEventRaised -= StopMusic;
        musicRelay.OnEventRaised -= PlayMusic;
    }
}
