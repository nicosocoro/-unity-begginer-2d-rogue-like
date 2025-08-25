using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public BoardManager BoardManager;
    public PlayerController PlayerController;
    public FoodManager FoodManager;
    public LevelManager LevelManager;
    public UIDocument uiMain;

    private TurnManager _turnManager;
    private Label _foodLabel;
    private Label _levelLabel;
    private VisualElement _uiGameOverPanel;


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

        _foodLabel = uiMain.rootVisualElement.Q<Label>("FoodLabel");
        _levelLabel = uiMain.rootVisualElement.Q<Label>("LevelLabel");
        _uiGameOverPanel = uiMain.rootVisualElement.Q<VisualElement>("GameOverPanel");
        _uiGameOverPanel.visible = false;

        BoardManager.OnFoodCreated += FoodManager.OnFoodCreated;
        FoodManager.OnNoMoreFoodLeft += OnGameOver;
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

        LevelManager.IncreaseLevel();
    }

    private void SpawnPlayer()
    {
        PlayerController.Spawn(BoardManager, BoardManager.GetInitialPosition());
    }

    private void UpdateLevelLabel()
    {
        _levelLabel.text = $"Level: {LevelManager.Level}";
    }

    private void OnGameOver()
    {
        _uiGameOverPanel.visible = true;
        PlayerController.gameObject.SetActive(false);
    }
}
