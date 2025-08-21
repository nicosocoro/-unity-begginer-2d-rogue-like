using UnityEngine;

public abstract class CellObject : MonoBehaviour
{
    public virtual void OnPlayerEntered() { }
}

public class TileObject : CellObject
{
    public override void OnPlayerEntered()
    {
        Debug.Log("Tile object entered!");
    }
}
