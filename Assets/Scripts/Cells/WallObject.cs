using UnityEngine;

public class WallObject : CellObject
{
    public override void OnInit(Vector2Int position)
    {
        // Initialize wall-specific properties if needed
        Debug.Log("Wall object initialized at position: " + position);
    }

    public override void OnPlayerEntered()
    {
        Debug.Log("Wall object entered!");
    }
}
