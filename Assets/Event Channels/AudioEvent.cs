using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Audio Event", menuName = "Event Channel/Audio Event")]
public class AudioChannel : ScriptableObject
{
    public UnityAction<AudioClip> OnEventRaised;

    public void RaiseEvent(AudioClip sound) {
        OnEventRaised?.Invoke(sound);
    }
}
