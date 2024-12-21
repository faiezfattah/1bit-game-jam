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
    [SerializeField] private SoundRelay relay;
    
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
        position.z = -10;
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
    private void PauseMusic(bool value) {
        if (value == true) { 
            musicSource.Pause(); 
        }
        else {
            musicSource.UnPause();
        }
    }

    private void StopMusic() {
        musicSource.Stop();
    }

    private void ProcessRelay(SoundEvent data) {
        if (data.Soundtype == SoundType.Music) {
            ProcessMusic(data);
            return;
        };
        ProcessSFX(data);
    }

    private void ProcessMusic(SoundEvent data) {
        switch (data.Type) {
            case SoundEventType.Play:
                PlayMusic(data.Audioclip);
                break;
            case SoundEventType.Stop:
                StopMusic();
                break;
            case SoundEventType.Pause:
                PauseMusic(true);
                break;
            case SoundEventType.Unpause:
                PauseMusic(true);
                break;
        }
    }

    private void ProcessSFX(SoundEvent data) {
        switch (data.Soundtype) {
            case SoundType.SFX:
                PlaySFX(data.Audioclip);
                break;
            case SoundType.LocalSFX:
                PlaySFX(data.Audioclip, data.Position);
                break;
        }
    }
    private void HandleDayOverSound() {
        PlaySFX(dayOverClip);
    }
    private void OnEnable() {
        relay.OnEventRaised += ProcessRelay;
    }    
    private void OnDisable() {
        relay.OnEventRaised -= ProcessRelay;
    }
}
