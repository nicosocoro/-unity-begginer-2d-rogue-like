using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    private Tilemap _tilemap;
    private Grid _grid;
    private CellData[,] _cells;
    private bool[,] _availableCells;

    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;
    public List<FoodObject> FoodPrefabs;
    public ObstacleObject ObstaclePrefab;
    public PlayerController Player;

    public event System.Action<FoodObject> OnFoodCreated;

    public void Initialize()
    {
        _tilemap = GetComponentInChildren<Tilemap>();
        _grid = GetComponentInChildren<Grid>();
        _cells = new CellData[Width, Height];
        _availableCells = new bool[Width, Height];

        Player.OnPlayerMoved += HandlePlayerMoved;

        BuildMap();
        GenerateObstacles();
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

        var currentCell = _cells[x, y];
        return currentCell.IsPassable && currentCell.ContainedObject?.OnPlayerTryingToEnter() != false;
    }

    public void SetTileToCell(Vector2Int position, Tile tile)
    {
        _tilemap.SetTile(new Vector3Int(position.x, position.y, 0), tile);
    }

    private void HandlePlayerMoved(Vector2Int newPosition)
    {
        _cells[newPosition.x, newPosition.y].ContainedObject?.OnPlayerEntered();
    }

    private void BuildMap()
    {
        var initialPosition = GetInitialPosition();
        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                bool isWallTile = x == 0 || y == 0 || x == Width - 1 || y == Height - 1;
                if (isWallTile)
                {
                    int wallTileNumber = Random.Range(0, WallTiles.Length);
                    _tilemap.SetTile(new Vector3Int(x, y, 0), WallTiles[wallTileNumber]);
                    _cells[x, y].IsPassable = false;
                    _availableCells[x, y] = false;
                }
                else
                {
                    int tileNumber = Random.Range(0, GroundTiles.Length);
                    _tilemap.SetTile(new Vector3Int(x, y, 0), GroundTiles[tileNumber]);
                    _cells[x, y].IsPassable = true;
                    _availableCells[x, y] = initialPosition != new Vector2Int(x, y);
                }
            }
        }
    }

    private void GenerateObstacles()
    {
        var createdCounter = 0;
        var obstacleInMapAmount = Random.Range(2, 5);
        var initialPosition = GetInitialPosition();

        while (createdCounter < obstacleInMapAmount)
        {
            Vector2Int cellPosition = GenerateRandomCell();

            if (initialPosition == cellPosition) continue;
            if (!_availableCells[cellPosition.x, cellPosition.y]) continue;

            ObstacleObject newObstacle = Instantiate(ObstaclePrefab);
            newObstacle.transform.position = new Vector3Int(cellPosition.x, cellPosition.y, 0);
            newObstacle.OnInit(cellPosition);
            var cell = new CellData
            {
                IsPassable = true,
                ContainedObject = newObstacle
            };

            _cells[cellPosition.x, cellPosition.y] = cell;
            _availableCells[cellPosition.x, cellPosition.y] = false;
            createdCounter++;
        }
    }

    private void GenerateFood()
    {
        var createdFoodCounter = 0;
        var foodInMapAmount = Random.Range(2, 5);
        var initialPosition = GetInitialPosition();

        while (createdFoodCounter < foodInMapAmount)
        {
            Vector2Int cellPosition = GenerateRandomCell();

            if (initialPosition == cellPosition) continue;
            if (!_availableCells[cellPosition.x, cellPosition.y]) continue;

            var foodPrefab = FoodPrefabs[Random.Range(0, FoodPrefabs.Count)];
            FoodObject newFood = Instantiate(foodPrefab);
            newFood.transform.position = new Vector3Int(cellPosition.x, cellPosition.y, 0);
            var cellData = new CellData
            {
                IsPassable = true,
                ContainedObject = newFood
            };

            _cells[cellPosition.x, cellPosition.y] = cellData;
            createdFoodCounter++;
            _availableCells[cellPosition.x, cellPosition.y] = false;
            OnFoodCreated?.Invoke(newFood);
        }
    }

    private Vector2Int GenerateRandomCell()
    {
        var x = Random.Range(1, Width - 1);
        var y = Random.Range(1, Height - 1);
        return new Vector2Int(x, y);
    }

    private struct CellData
    {
        public bool IsPassable;
        public CellObject ContainedObject;
    }
}
