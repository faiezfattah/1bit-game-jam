using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BuildData Event", menuName = "Event Channel/BuildData Event")]
public class BuildDataChannel : ScriptableObject
{
    public UnityAction<Build> OnEventRaised;

    public void RaiseEvent(Build build)
    {
        OnEventRaised?.Invoke(build);
    }
}
