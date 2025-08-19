using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardManager BoardManager;
    public PlayerController PlayerController;

    private TurnManager _turnManager;

    void Awake()
    {
        _turnManager = new TurnManager(PlayerController);
    }

    void Start()
    {
        BoardManager.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
