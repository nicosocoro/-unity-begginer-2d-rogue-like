using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleObject : CellObject
{
    private int _resistance = 2;

    public Tile WallTile;

    public override void OnInit(Vector2Int position)
    {
        base.OnInit(position);
        GameManager.Instance.BoardManager.SetTileToCell(position, WallTile);
    }

    public override bool OnPlayerTryingToEnter()
    {
        _resistance--;

        // change asset based on damage

        if (_resistance <= 0)
        {
            OnDestroy();
            return true;
        }

        return false;
    }
}
