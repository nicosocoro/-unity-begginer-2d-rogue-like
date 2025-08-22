using UnityEngine;

public class TileObject : CellObject
{
    public override void OnPlayerEntered()
    {
        Debug.Log("Tile object entered!");
    }
}
