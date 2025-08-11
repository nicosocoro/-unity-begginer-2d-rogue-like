using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    private Tilemap _tilemap;
    private Grid _grid;
    private CellData[,] _cells;

    public PlayerController PlayerController;
    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;

    // Start is called before the first frame update
    void Start()
    {
        _tilemap = GetComponentInChildren<Tilemap>();
        _grid = GetComponentInChildren<Grid>();
        _cells = new CellData[Width, Height];

        BuildMap();
        SpawnPlayer();
    }

    public Vector3Int GetCellPosition(Vector2Int cell)
    {
        return _grid.WorldToCell((Vector3Int)cell);
    }

    private void BuildMap()
    {
        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                {
                    int wallTileNumber = Random.Range(0, WallTiles.Length);
                    _tilemap.SetTile(new Vector3Int(x, y, 0), WallTiles[wallTileNumber]);
                    _cells[x, y].IsPassable = false;
                }
                else
                {
                    int tileNumber = Random.Range(0, GroundTiles.Length);
                    _tilemap.SetTile(new Vector3Int(x, y, 0), GroundTiles[tileNumber]);
                    _cells[x, y].IsPassable = true;
                }
            }
        }
    }

    private void SpawnPlayer()
    {
        PlayerController.Spawn(this, new Vector2Int(1, 1));
    }

    private struct CellData
    {
        public bool IsPassable;
    }
}
