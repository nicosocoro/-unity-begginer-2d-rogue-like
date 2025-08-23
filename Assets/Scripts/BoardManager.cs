using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
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
    public Tile ExitTile;
    public List<FoodObject> FoodPrefabs;
    public ObstacleObject ObstaclePrefab;
    public ExitObject ExitPrefab;
    public PlayerController Player;

    public event System.Action<FoodObject> OnFoodCreated;

    public void Initialize()
    {
        _tilemap = GetComponentInChildren<Tilemap>();
        _grid = GetComponentInChildren<Grid>();
        _cells = new CellData[Width, Height];
        _availableCells = new bool[Width, Height];

        Player.OnPlayerMoved += HandlePlayerMoved;

        BuildMap(fromScratch: true);
        GenerateObstacles();
        GenerateFood();
    }

    public void GenerateNewLevel()
    {
        ClearCurrentLevel();
        BuildMap(fromScratch: true);
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

    public void OnFreeCell(Vector2Int position)
    {
        Debug.Log($"Freeing cell at {position}");
        _availableCells[position.x, position.y] = true;
        _cells[position.x, position.y] = new CellData { IsPassable = true, ContainedObject = null };

        SetGroundTileTo(position.x, position.y);
    }

    private void HandlePlayerMoved(Vector2Int newPosition)
    {
        _cells[newPosition.x, newPosition.y].ContainedObject?.OnPlayerEntered();
    }

    private void BuildMap(bool fromScratch)
    {
        var initialPosition = GetInitialPosition();
        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                var isExitTile = x == Width - 2 && y == Height - 1;
                if (isExitTile && fromScratch)
                {
                    ExitObject exitPrefab = Instantiate(ExitPrefab);
                    exitPrefab.transform.position = new Vector3Int(x, y, 0);
                    exitPrefab.OnInit(new Vector2Int(x, y));
                    _cells[x, y] = new CellData { IsPassable = true, ContainedObject = exitPrefab };
                    _availableCells[x, y] = false;
                    _tilemap.SetTile(new Vector3Int(x, y, 0), ExitTile);
                    continue;
                }

                if (IsWallTile(x, y) && fromScratch)
                {
                    int wallTileNumber = Random.Range(0, WallTiles.Length);
                    _tilemap.SetTile(new Vector3Int(x, y, 0), WallTiles[wallTileNumber]);
                    _cells[x, y].IsPassable = false;
                    _availableCells[x, y] = false;
                    continue;
                }

                SetGroundTileTo(y, x);
                _cells[x, y].IsPassable = true;
                _availableCells[x, y] = initialPosition != new Vector2Int(x, y);
            }
        }
    }

    private void SetGroundTileTo(int x, int y)
    {
        int tileNumber = Random.Range(0, GroundTiles.Length);
        _tilemap.SetTile(new Vector3Int(x, y, 0), GroundTiles[tileNumber]);
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
            newFood.OnInit(cellPosition);
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

    private bool IsWallTile(int x, int y)
    {
        return x == 0 || y == 0 || x == Width - 1 || y == Height - 1;
    }

    private void ClearCurrentLevel()
    {
        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                if (IsWallTile(x, y)) continue;

                var cell = _cells[x, y];
                if (cell.ContainedObject != null)
                {
                    Destroy(cell.ContainedObject.gameObject);
                }
                _cells[x, y] = new CellData();
                _availableCells[x, y] = true;
                _tilemap.SetTile(new Vector3Int(x, y, 0), null);
            }
        }
    }

    private struct CellData
    {
        public bool IsPassable;
        public CellObject ContainedObject;
    }
}
