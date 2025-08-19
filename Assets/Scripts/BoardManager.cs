using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    private Tilemap _tilemap;
    private Grid _grid;
    private CellData[,] _cells;

    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;
    public GameObject foodPrefab;

    public void Initialize()
    {
        _tilemap = GetComponentInChildren<Tilemap>();
        _grid = GetComponentInChildren<Grid>();
        _cells = new CellData[Width, Height];

        BuildMap();
        GenerateFood();
    }

    public Vector2Int GetInitialPosition()
    { 
        return new Vector2Int(1, 1);
    }

    public Vector3Int GetCellPosition(Vector2Int cell)
    {
        return _grid.WorldToCell((Vector3Int)cell);
    }

    public bool IsCellAvailableForPlayer(Vector2Int cell)
    {
        var x = cell.x;
        var y = cell.y;

        if (x < 0 || y < 0 || x >= Width || y >= Height)
        {
            return false;
        }

        return _cells[x, y].IsPassable;
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

    private void GenerateFood()
    {
        var createdFoodCounter = 0;
        var foodInMapAmount = Random.Range(2, 5);
        Vector2Int[] createdFoodPositions = new Vector2Int[foodInMapAmount];
        var initialPosition = GetInitialPosition();

        while (createdFoodCounter < foodInMapAmount)
        {
            var x = Random.Range(1, Width - 1);
            var y = Random.Range(1, Height - 1);
            var cellPosition = new Vector2Int(x, y);
            
            if (initialPosition == cellPosition) continue;
            if (createdFoodPositions.Contains(cellPosition)) continue;

            var newFood = Instantiate(foodPrefab);
            newFood.transform.position = new Vector3Int(x, y, 0);
            var cell = new CellData
            {
                IsPassable = true,
                ContainedObject = newFood
            };

            _cells[x, y] = cell;
            createdFoodPositions[createdFoodCounter] = cellPosition;
            createdFoodCounter++;
        }
    }

    private struct CellData
    {
        public bool IsPassable;
        public GameObject ContainedObject;
    }
}
