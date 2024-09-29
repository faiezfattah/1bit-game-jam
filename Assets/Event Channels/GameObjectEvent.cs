using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "GameObject Event", menuName = "Event Channel/GameObject Event")]
public class GameObjectChannel : ScriptableObject
{
    public UnityAction<GameObject> OnEvenRaised;

    public void RaiseEvent(GameObject obj)
    {
        OnEvenRaised?.Invoke(obj);
    }
}
