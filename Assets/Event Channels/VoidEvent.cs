using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Void Event", menuName = "Event Channel/Void Event")]
public class VoidChannel : ScriptableObject
{
    public UnityAction OnEvenRaised;

    public void RaiseEvent()
    {
        OnEvenRaised?.Invoke();
    }
}
