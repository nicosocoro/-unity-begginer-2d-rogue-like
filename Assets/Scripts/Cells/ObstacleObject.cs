using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleObject : CellObject
{
    private int _lifes = 2;

    public Tile WallTile;

    public override void OnInit(Vector2Int position)
    {
        base.OnInit(position);
        GameManager.Instance.BoardManager.SetTileToCell(position, WallTile);
    }

    public override bool OnPlayerTryingToEnter()
    {
        _lifes--;

        if (_lifes <= 0)
        {
            OnDestroy();
            return true;
        }

        return false;
    }
}
