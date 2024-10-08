using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Bool Event", menuName = "Event Channel/Bool Event")]
public class BoolChannel : ScriptableObject
{
    public UnityAction<bool> OnEventRaised;

    public void RaiseEvent(bool value) {
        OnEventRaised?.Invoke(value);
    }
}
