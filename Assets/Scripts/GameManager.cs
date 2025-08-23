using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public BoardManager BoardManager;
    public PlayerController PlayerController;
    public FoodManager FoodManager;
    public UIDocument uiFood;

    private TurnManager _turnManager;
    private Label _foodLabel;
    private Label _levelLabel;

    private int _currentLevel = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _turnManager = new TurnManager(PlayerController);

        _turnManager.OnTurnFinished += OnTurnFinished;

        _foodLabel = uiFood.rootVisualElement.Q<Label>("FoodLabel");
        _levelLabel = uiFood.rootVisualElement.Q<Label>("LevelLabel");

        BoardManager.OnFoodCreated += FoodManager.OnFoodCreated;
    }

    void Start()
    {
        BoardManager.Initialize();
        SpawnPlayer();
    }

    void Update()
    {
        UpdateFoodLabel();
        UpdateLevelLabel();
    }

    void UpdateFoodLabel()
    {
        _foodLabel.text = $"Food: {FoodManager.Food}";
    }

    public void OnTurnFinished()
    {
        FoodManager.ConsumeFood();
    }

    public void OnLevelFinished()
    {
        BoardManager.GenerateNewLevel();
        FoodManager.ResetFood();
        SpawnPlayer();

        _currentLevel++;
    }

    private void SpawnPlayer()
    {
        PlayerController.Spawn(BoardManager, BoardManager.GetInitialPosition());
    }

    private void UpdateLevelLabel()
    {
        _levelLabel.text = $"Level: {_currentLevel}";
    }
}
