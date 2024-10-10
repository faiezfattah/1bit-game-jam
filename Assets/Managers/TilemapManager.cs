using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    [SerializeField] private VectorIntChannel tileRemovalRelay;
    [SerializeField] private Tilemap resourceTilemap;
    [SerializeField] private int resourcePointPerTile = 500;

    private Dictionary<Vector3Int, int> tileMapResource = new Dictionary<Vector3Int, int>();
    private void Start() {
        if (resourceTilemap == null) resourceTilemap = GetComponent<Tilemap>();
    }
    public int GetResources(Vector3Int pos, int amount) {
        RecordResourceChanges(pos, amount);
        return amount;
    }
    private void RecordResourceChanges(Vector3Int pos, int decreaseAmount) {
        if (tileMapResource.ContainsKey(pos) == false) {
            tileMapResource.Add(pos, resourcePointPerTile);
        }

        tileMapResource[pos] -= decreaseAmount;

        if (tileMapResource[pos] <= 0 ) { 
            RemoveTile(pos);
        }
    }
    private void RemoveTile(Vector3Int position) {
        resourceTilemap.SetTile(position, null);
        tileMapResource.Remove(position);

        Debug.Log("removing tile at: " + position);
    }
    private void OnEnable() {
        tileRemovalRelay.OnEventRaised += RemoveTile;
    }

    private void OnDisable() {
        tileRemovalRelay.OnEventRaised -= RemoveTile;
    }
}
