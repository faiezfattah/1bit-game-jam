using UnityEngine;

public class TurretSelect : MonoBehaviour
{
    [SerializeField] private BuildDataChannel relay;
    public void SendBuild(GameObject build)
    {
        relay.RaiseEvent(build.GetComponent<Build>());
    }
}
