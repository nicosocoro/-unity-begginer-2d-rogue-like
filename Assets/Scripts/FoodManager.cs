using UnityEngine;

public class FoodManager : MonoBehaviour
{
    private int _food = 10;
    public int Food { get => _food; private set { _food = value; } }

    public void ConsumeFood()
    {
        _food--;
    }

    public void IncreaseFood(int amount)
    {
        _food += amount;
    }

    public void OnFoodCreated(FoodObject food)
    {
        food.OnFoodConsumed += OnFoodConsumed;
    }
    
    private void OnFoodConsumed(FoodObject food)
    {
        IncreaseFood(food.FoodEarnedOnEntered);
        food.OnFoodConsumed -= OnFoodConsumed;
    }
}
