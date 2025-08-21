using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public BoardManager BoardManager;
    public PlayerController PlayerController;
    public FoodManager FoodManager;
    public UIDocument uiFood;

    private TurnManager _turnManager;
    private Label _uiLabel;

    void Awake()
    {
        _turnManager = new TurnManager(PlayerController);

        _turnManager.OnTurnFinished += OnTurnFinished;
        _uiLabel = uiFood.rootVisualElement.Q<Label>("FoodLabel");

        BoardManager.OnFoodCreated += FoodManager.OnFoodCreated;
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
        _uiLabel.text = $"Food: {FoodManager.Food}";
    }

    public void OnTurnFinished()
    {
        FoodManager.ConsumeFood();
    }
}
