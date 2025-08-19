using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public BoardManager BoardManager;
    public PlayerController PlayerController;
    public UIDocument uiFood;

    private TurnManager _turnManager;
    private FoodManager _foodManager;
    private Label _uiLabel;

    void Awake()
    {
        _turnManager = new TurnManager(PlayerController);
        _foodManager = new FoodManager();

        _turnManager.OnTurnFinished += OnTurnFinished;
        _uiLabel = uiFood.rootVisualElement.Q<Label>("FoodLabel");
    }

    void Start()
    {

        BoardManager.Initialize();
        PlayerController.Spawn(BoardManager, BoardManager.GetInitialPosition());
    }

    void Update()
    {
        UpdateFoodLabel();
    }

    void UpdateFoodLabel()
    {
        _uiLabel.text = $"Food: {_foodManager.Food}";
    }

    public void OnTurnFinished()
    {
        _foodManager.ConsumeFood();
    }
}
