using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Float Event", menuName = "Event Channel/Float Event")]
public class FloatEvent : ScriptableObject
{
    public UnityAction<float> OnEventRaised;

    public void RaiseEvent(float value) {
        OnEventRaised?.Invoke(value);
    }
}
