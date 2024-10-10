using Mono.Cecil;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Miner : Build
{
    [SerializeField] private PlayerEconomy economy;
    [SerializeField] private ResourceTile.resourceType minerType = ResourceTile.resourceType.coal;
    [SerializeField] private int point = 0;
    [SerializeField] private int gatherPointPerTile = 1;
    [SerializeField] private int maxGatherPoint = 5;


    private static Tilemap resourceTilemap;
    private static TilemapManager tileManager;
    private float gatherTimer;
    void Start()
    {
        resourceTilemap = GameObject.FindGameObjectWithTag("ResourceMap").GetComponent<Tilemap>();
        tileManager = resourceTilemap.GetComponent<TilemapManager>();
        gatherTimer = data.attackInterval;
    }

    void Update()
    {
        gatherTimer -= Time.deltaTime;
        if (gatherTimer <= 0) {
            GatherResources();
            gatherTimer = data.attackInterval;
        }
        if (point > maxGatherPoint) {
            point -= maxGatherPoint;
            AddToEconomy(1);
        }
    }
    private void AddToEconomy(int amount) {
        switch (minerType) {
            case ResourceTile.resourceType.coal:
                economy.AddCoal(amount); 
                break;
            case ResourceTile.resourceType.iron:
                economy.AddIron(amount);
                break;
        }
    }
    void GatherResources()
    {
        Vector3Int[] neighborTiles = GetNeighborTiles();
        foreach (Vector3Int tilePos in neighborTiles)
        {
            ResourceTile tile = resourceTilemap.GetTile(tilePos) as ResourceTile;

            if (tile != null && tile.type == minerType)
            {
                tileManager.GetResources(tilePos, gatherPointPerTile);
                Debug.Log("Getting resources on: " + tilePos);
            }
        }
    }
    Vector3Int[] GetNeighborTiles()
    {
        Vector3Int centerTile = resourceTilemap.WorldToCell(transform.position);
        int radius = Mathf.CeilToInt(data.range);
        Vector3Int[] neighbors = new Vector3Int[(radius * 2 + 1) * (radius * 2 + 1)];
        int index = 0;
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                neighbors[index] = new Vector3Int(centerTile.x + x, centerTile.y + y, centerTile.z);
                index++;
            }
        }
        return neighbors;
    }
}
