using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "PlayerBuild", menuName = "Player/Build")]
public class PlayerBuild : ScriptableObject
{
    public Dictionary<Vector3Int, Turret> turretPlacement = new Dictionary<Vector3Int, Turret>();
    private void OnEnable()
    {
        // debugging purposes
        turretPlacement.Clear();
    }

    public bool AddTurret(Vector3Int position, Turret turret)
    {
        if (turretPlacement.ContainsKey(position) == false)
        {
            turretPlacement[position] = turret;
            return true;
        }
        else return false;
    }
    public bool CheckTurret(Vector3Int position)
    {
        return turretPlacement.ContainsKey(position);
    }
    public Turret GetTurret(Vector3Int position)
    {
        if (turretPlacement.ContainsKey(position))
        {
            return turretPlacement[position];
        }
        else return null;
    }
    public void RemoveTurret(Vector3Int position)
    {
        turretPlacement.Remove(position);
    }
}
