using UnityEngine;

public class TurnManager
{
    public int Turn { get; private set; } = 0;

    private PlayerController _player;

    public TurnManager(PlayerController player)
    {
        _player = player;

        _player.OnTurnFinished += OnPlayerMoved;
    }

    public void OnPlayerMoved()
    { 
        Turn++;
        Debug.Log($"Turn {Turn} finished.");
    }
}
