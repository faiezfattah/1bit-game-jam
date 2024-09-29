using UnityEngine;

public class TurretSelect : MonoBehaviour
{
    [SerializeField] private GameObjectChannel relay;
    public void SendTurret(GameObject turret)
    {
        relay.RaiseEvent(turret);
    }
}
