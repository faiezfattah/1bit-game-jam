using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VectorInt Event", menuName = "Event Channel/VectorInt Event")]
public class VectorIntChannel : ScriptableObject {

    public UnityAction<Vector3Int> OnEventRaised;
    public void RaiseEvent(Vector3Int value) {
        OnEventRaised?.Invoke(value);
    }
}
