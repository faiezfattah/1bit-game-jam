using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics.Tracing;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private PlayerGameTime gameTime;

    [Header("Sources----")]
    [SerializeField] private AudioSource sfxSourceGLobal;
    [SerializeField] private AudioSource musicSource;

    [Header("Relays----")]
    [SerializeField] private AudioChannel sfxRelay;
    [SerializeField] private AudioChannel musicRelay;
    [SerializeField] private BoolChannel toggleMusicPlay;
    [SerializeField] private LocalAudioEvent localAudioRelay;

    [Header("Sound pool")]
    [SerializeField] private int maxSfxPool = 100;
    protected ObjectPool<AudioSource> localSfxPool;

    [Header("Sounds----")]
    [SerializeField] private AudioClip dayOverClip;
    private void Awake() {
        if (sfxSourceGLobal == null)
            sfxSourceGLobal = gameObject.AddComponent<AudioSource>();

        if (musicSource == null) {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }

        localSfxPool = new ObjectPool<AudioSource>(() => {
            GameObject sfxObject = new GameObject("localSFX");
            AudioSource sfxSource = sfxObject.AddComponent<AudioSource>();
            sfxSource.spatialBlend = 1;
            sfxSource.playOnAwake = false;
            sfxSource.volume = 2;
            return sfxSource;
        }, sfxSource => {
            sfxSource.gameObject.SetActive(true);
        }, sfxSource => {
            sfxSource.gameObject.SetActive(false);
        }, sfxSource => {
            Destroy(sfxSource);
        }, false, 2, maxSfxPool);

    }
    private void PlaySFX(AudioClip clip) {
        sfxSourceGLobal.PlayOneShot(clip);
    }
    private void PlaySFX(AudioClip clip, Vector3 position) {
        AudioSource sfx = localSfxPool.Get();
        sfx.transform.position = position;
        sfx.clip = clip;
        sfx.Play();
        StartCoroutine(ReturnToPoolAfterPlay(sfx));
    }
    private IEnumerator ReturnToPoolAfterPlay(AudioSource source) {
        yield return new WaitForSeconds(source.clip.length);
        if (source.isPlaying) source.Stop();
        localSfxPool?.Release(source);
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
    private void HandleDayOverSound() {
        PlaySFX(dayOverClip);
    }
    private void OnEnable() {
        sfxRelay.OnEventRaised += PlaySFX;
        toggleMusicPlay.OnEventRaised += StopMusic;
        musicRelay.OnEventRaised += PlayMusic;
        localAudioRelay.OnEventRaised += PlaySFX;
        gameTime.onDayOver += HandleDayOverSound;
    }    
    private void OnDisable() {
        sfxRelay.OnEventRaised -= PlaySFX;
        toggleMusicPlay.OnEventRaised -= StopMusic;
        musicRelay.OnEventRaised -= PlayMusic;
        localAudioRelay.OnEventRaised -= PlaySFX;
        gameTime.onDayOver -= HandleDayOverSound;
    }
}
