using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleObject : CellObject
{
    public Tile WallTile;
    public override void OnInit(Vector2Int position)
    {
        GameManager.Instance.BoardManager.SetTileToCell(position, WallTile);
    }
}
