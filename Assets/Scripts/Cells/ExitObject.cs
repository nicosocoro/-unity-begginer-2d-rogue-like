using UnityEngine;

public class ExitObject : CellObject
{
    public override void OnPlayerEntered()
    {
        Debug.Log("Player has entered the exit object.");
    }
}
