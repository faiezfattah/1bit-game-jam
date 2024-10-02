using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "PlayerBuild", menuName = "Player/Build")]
public class PlayerBuild : ScriptableObject
{
    public Dictionary<Vector3Int, GameObject> buildPlacement = new Dictionary<Vector3Int, GameObject>();
    private void OnEnable()
    {
        // debugging purposes
        buildPlacement.Clear();
    }

    public bool AddBuild(Vector3Int position, GameObject build)
    {
        if (buildPlacement.ContainsKey(position) == false)
        {
            buildPlacement[position] = build;
            return true;
        }
        else return false;
    }
    public bool CheckTurret(Vector3Int position)
    {
        return buildPlacement.ContainsKey(position);
    }
    public GameObject GetBuild(Vector3Int position)
    {
        if (buildPlacement.ContainsKey(position))
        {
            return buildPlacement[position];
        }
        else return null;
    }
    public void RemoveTurret(Vector3Int position)
    {
        Destroy(buildPlacement[position]);
        buildPlacement.Remove(position);
    }
}
