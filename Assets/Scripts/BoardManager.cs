using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    private Tilemap _tilemap;
    private CellData[,] _cells;

    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;

    // Start is called before the first frame update
    void Start()
    {
        _tilemap = GetComponentInChildren<Tilemap>();
        _cells = new CellData[Width, Height];
        BuildWalls();
        BuildMap();
    }

    private void BuildWalls()
    {
        for (int x = 0; x < Height; ++x)
        {
            int wallTileNumber = Random.Range(0, WallTiles.Length);
            _tilemap.SetTile(new Vector3Int(x, 0, 0), WallTiles[wallTileNumber]);
            _cells[x, 0].IsPassable = false;
        }

        for (int y = 0; y < Height; ++y)
        {
            int wallTileNumber = Random.Range(0, WallTiles.Length);
            _tilemap.SetTile(new Vector3Int(0, y, 0), WallTiles[wallTileNumber]);
            _cells[0, y].IsPassable = false;
        }
    }

    private void BuildMap()
    {
        for (int y = 1; y < Height; ++y)
        {
            for (int x = 1; x < Width; ++x)
            {
                int tileNumber = Random.Range(0, GroundTiles.Length);
                _tilemap.SetTile(new Vector3Int(x, y, 0), GroundTiles[tileNumber]);
                _cells[x, y].IsPassable = true;
            }
        }
    }

    private struct CellData
    {
        public bool IsPassable;
    }
}
