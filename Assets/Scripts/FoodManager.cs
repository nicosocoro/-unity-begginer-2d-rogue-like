public class FoodManager
{
    private int _food = 100;

    public int Food { get => _food; private set { _food = value; } }

    public void ConsumeFood()
    {
        _food--;
    }
}
