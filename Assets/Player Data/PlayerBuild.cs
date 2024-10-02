using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "PlayerBuild", menuName = "Player/Build")]
public class PlayerBuild : ScriptableObject
{
    public Dictionary<Vector3Int, BuildData> buildPlacement = new Dictionary<Vector3Int, BuildData>();
    private void OnEnable()
    {
        // debugging purposes
        buildPlacement.Clear();
    }

    public bool AddBuild(Vector3Int position, GameObject build)
    {
        if (buildPlacement.ContainsKey(position) == false)
        {
            buildPlacement[position] = build.GetComponent<Build>().data;
            return true;
        }
        else return false;
    }
    public bool CheckTurret(Vector3Int position)
    {
        return buildPlacement.ContainsKey(position);
    }
    public BuildData GetBuild(Vector3Int position)
    {
        if (buildPlacement.ContainsKey(position))
        {
            return buildPlacement[position];
        }
        else return null;
    }
    public void RemoveTurret(Vector3Int position)
    {
        buildPlacement.Remove(position);
    }
}
