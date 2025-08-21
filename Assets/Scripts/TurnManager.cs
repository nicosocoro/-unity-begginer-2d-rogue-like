using System;
using UnityEngine;

public class TurnManager
{
    public int Turn { get; private set; } = 0;

    private readonly PlayerController _player;

    public event Action OnTurnFinished;

    public TurnManager(PlayerController player)
    {
        _player = player;

        _player.OnPlayerMoved += FinishTurn;
    }

    private void FinishTurn(Vector2Int newPosition)
    {
        Turn++;
        OnTurnFinished?.Invoke();
        
        Debug.Log($"Turn {Turn} finished.");
    }
}
