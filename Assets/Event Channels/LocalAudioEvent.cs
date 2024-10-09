using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Local Audio Event", menuName = "Event Channel/Local Audio Event")]
public class LocalAudioEvent : ScriptableObject
{
    public UnityAction<AudioClip, Vector3> OnEventRaised;

    public void RaiseEvent(AudioClip clip, Vector3 position) {
        OnEventRaised?.Invoke(clip, position);
    }
}
