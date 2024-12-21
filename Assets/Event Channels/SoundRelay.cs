using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SoundRelay", menuName = "Event Channel/Sound Relay")]
public class SoundRelay : ScriptableObject {
    public UnityAction<SoundEvent> OnEventRaised;

    public void RaiseEvent(SoundEvent data) {
        OnEventRaised?.Invoke(data);
    }
}

public struct SoundEvent {
    public AudioClip      Audioclip;
    public Vector2        Position;
    public SoundEventType Type;
    public SoundType      Soundtype;
}

public enum SoundEventType {
    Play,
    Pause,
    Unpause,
    Stop,
}

public enum SoundType {
    Music,
    SFX,
    LocalSFX
}