using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    private Tilemap m_Tilemap;

    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;

    // Start is called before the first frame update
    void Start()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        BuildWalls();
        BuildMap();
    }

    private void BuildWalls()
    {
        for (int x = 0; x < Height; ++x)
        {
            int wallTileNumber = Random.Range(0, WallTiles.Length);
            m_Tilemap.SetTile(new Vector3Int(x, 0, 0), WallTiles[wallTileNumber]);
        }

        for (int y = 0; y < Height; ++y)
        {
            int wallTileNumber = Random.Range(0, WallTiles.Length);
            m_Tilemap.SetTile(new Vector3Int(0, y, 0), WallTiles[wallTileNumber]);
        }
    }

    private void BuildMap()
    {
        for (int y = 1; y < Height; ++y)
        {
            for (int x = 1; x < Width; ++x)
            {
                int tileNumber = Random.Range(0, GroundTiles.Length);
                m_Tilemap.SetTile(new Vector3Int(x, y, 0), GroundTiles[tileNumber]);
            }
        }
    }
}
