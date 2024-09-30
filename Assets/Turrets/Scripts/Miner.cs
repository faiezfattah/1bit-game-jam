using UnityEngine;
using UnityEngine.Tilemaps;

public class IronMiner : MonoBehaviour
{
    [SerializeField] private PlayerEconomy economy;

    public float gatherRadius = 1f;
    public float gatherInterval = 1f;
    public int gatherAmount = 1;

    private static Tilemap resourceTilemap;
    private float gatherTimer;
    void Start()
    {
        resourceTilemap = GameObject.FindGameObjectWithTag("ResourceMap").GetComponent<Tilemap>();
        gatherTimer = gatherInterval;
    }

    void Update()
    {
        gatherTimer -= Time.deltaTime;
        if (gatherTimer <= 0)
        {
            GatherResources();
            gatherTimer = gatherInterval;
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
                    case ResourceTile.resourceType.wood:
                        Debug.Log("wood detected");
                        break;
                    case ResourceTile.resourceType.iron:
                        Debug.Log("iron detected");
                        economy.AddIron(gatherAmount);
                        break;
                }
            }
        }
    }
    Vector3Int[] GetNeighborTiles()
    {
        Vector3Int centerTile = resourceTilemap.WorldToCell(transform.position);
        int radius = Mathf.CeilToInt(gatherRadius);
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
