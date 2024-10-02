using UnityEngine;
using UnityEngine.Tilemaps;

public class IronMiner : Build
{
    [SerializeField] private PlayerEconomy economy;
    [SerializeField] private int gatherAmount = 1;

    private static Tilemap resourceTilemap;
    private float gatherTimer;
    void Start()
    {
        resourceTilemap = GameObject.FindGameObjectWithTag("ResourceMap").GetComponent<Tilemap>();
        gatherTimer = data.attackInterval;
    }

    void Update()
    {
        gatherTimer -= Time.deltaTime;
        if (gatherTimer <= 0)
        {
            GatherResources();
            gatherTimer = data.attackInterval;
        }
    }
    void GatherResources()
    {
        Vector3Int[] neighborTiles = GetNeighborTiles();
        foreach (Vector3Int tilePos in neighborTiles)
        {
            ResourceTile tile = resourceTilemap.GetTile(tilePos) as ResourceTile;
            if (tile != null)
            {
                switch (tile.type)
                {
                    case ResourceTile.resourceType.coal:
                        economy.AddIron(gatherAmount);
                        break;
                    case ResourceTile.resourceType.iron:
                        economy.AddIron(gatherAmount);
                        break;
                }
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
