using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    [SerializeField] private VectorIntChannel tileRemovalRelay;
    [SerializeField] private Tilemap resourceTilemap;

    private Dictionary<Vector3Int, int> tileMapResource = new Dictionary<Vector3Int, int>();
    private void Start() {
        if (resourceTilemap == null) resourceTilemap = GetComponent<Tilemap>();

        //resourceTilemap.GetTilesRangeNonAlloc(new Vector3Int(-50, 50), new Vector3(50, -50), tileMapResource.Keys)
    }
    public int GetResources(int amount) {
        return amount;
    }
    private void RemoveTile(Vector3Int position) {
        resourceTilemap.SetTile(position, null);
        Debug.Log("removing tile at: " + position);
    }
    private void OnEnable() {
        tileRemovalRelay.OnEventRaised += RemoveTile;
    }

    private void OnDisable() {
        tileRemovalRelay.OnEventRaised -= RemoveTile;
    }
}
