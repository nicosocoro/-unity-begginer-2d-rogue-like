using System;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    private int _initialFood = 10;
    private int _food;
    public int Food { get => _food; private set { _food = value; } }

    public event Action OnNoMoreFoodLeft;

    public FoodManager()
    {
        ResetFood();
    }

    public void ResetFood()
    {
        _food = _initialFood;
    }

    public void ConsumeFood()
    {
        _food--;

        if (_food <= 0)
        {
            OnNoMoreFoodLeft?.Invoke();
        }
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
