using UnityEngine;

public class FoodObject : CellObject
{
    public override void OnPlayerEntered()
    {
        Debug.Log("Food eaten!");
        Destroy(gameObject);
    }
}
