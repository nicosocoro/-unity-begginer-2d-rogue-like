public class FoodObject : CellObject
{
    public int FoodEarnedOnEntered;

    public event System.Action<FoodObject> OnFoodConsumed;

    public override void OnPlayerEntered()
    {
        OnFoodConsumed?.Invoke(this);
        Destroy(gameObject);
    }
}
