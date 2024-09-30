using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Resource Tile", menuName = "Tilemap/Resource")]
public class ResourceTile : TileBase
{
    public enum resourceType { iron, wood }
    public resourceType type;
    public Sprite sprite;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = sprite;
    }
}
