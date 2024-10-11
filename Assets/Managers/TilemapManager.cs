using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    [SerializeField] private PlayerBuild build;
    [Header("relays --- ")]
    [SerializeField] private VectorIntChannel tileRemovalRelay;
    [SerializeField] private VoidChannel resetRelay;

    [SerializeField] private Tilemap resourceTilemap;

    private void Start() {
        if (resourceTilemap == null) resourceTilemap = GetComponent<Tilemap>();
    }
    public int GetResources(Vector3Int pos, int amount) {
        if (build.TrackResourceChanges(pos, amount))
            return amount;
        else RemoveTile(pos);
        return 0;

    }
    private void RemoveTile(Vector3Int position) {
        resourceTilemap.SetTile(position, null);
        build.tileResource.Remove(position);

        Debug.Log("removing tile at: " + position);
    }
    private void RebuildTileMap() {
        foreach (Vector3Int Key in build.tileResource.Keys) {
            if (build.tileResource[Key] <= 0 || build.tileResource == null) {
                RemoveTile(Key);
            }
        }
    }
    private void HandleReset() {
        build.ResetTileResource();
    }
    private void OnEnable() {
        tileRemovalRelay.OnEventRaised += RemoveTile;
        resetRelay.OnEvenRaised += HandleReset;
    }

    private void OnDisable() {
        tileRemovalRelay.OnEventRaised -= RemoveTile;
        resetRelay.OnEvenRaised -= HandleReset;
    }
}
