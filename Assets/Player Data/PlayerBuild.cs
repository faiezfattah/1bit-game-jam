using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "PlayerBuild", menuName = "Player/Build")]
public class PlayerBuild : ScriptableObject
{
    public int maxTowerHealth = 100;
    public int currentTowerHealth;
    public int resourcePointPerTile = 200;

    public Dictionary<Vector3Int, BuildData> buildPlacement = new Dictionary<Vector3Int, BuildData>();
    public Dictionary<Vector3Int, int> tileResource = new Dictionary<Vector3Int, int>();
    private void OnEnable()
    {
        // debugging purposes
        buildPlacement.Clear();
    }

    public void AddBuild(Vector3Int position, GameObject build)
    {
        if (buildPlacement.ContainsKey(position) == false)
        {
            buildPlacement[position] = build.GetComponent<Build>().data;
        }
    }
    public void UpdateBuild(Vector3Int position, GameObject build) {
        if (buildPlacement.ContainsKey(position)) {
            buildPlacement[position] = build.GetComponent<Build>().data;
        }
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
    public void RemoveBuild(Vector3Int position)
    {
        buildPlacement.Remove(position);
    }
    public void GetBuildsDictionary() {
        foreach (Vector3Int Key in buildPlacement.Keys) {
            Debug.Log("manual key lookup: " + buildPlacement[Key]);
            Debug.Log("GetBuild Method" + GetBuild(Key));
        }
    }
    public void ResetBuild() {
        buildPlacement.Clear();
        currentTowerHealth = maxTowerHealth;
    }
    public void ResetTileResource() {
        tileResource.Clear();
    }
    
    /// <summary>
    /// Return if point succesfully gathered
    /// false: need tile removal
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="decreaseAmount"></param>
    /// <returns></returns>
    public bool TrackResourceChanges(Vector3Int pos, int decreaseAmount) {
        if (tileResource.ContainsKey(pos) == false) {
            tileResource.Add(pos, resourcePointPerTile);
        }

        tileResource[pos] -= decreaseAmount;

        if (tileResource[pos] <= 0) {
            return false;
        }
        return true;
    }
}
