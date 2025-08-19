using UnityEngine;

public class TurnManager
{
    public int Turn { get; private set; } = 0;

    private readonly PlayerController _player;

    public TurnManager(PlayerController player)
    {
        _player = player;

        _player.OnPlayerMoved += FinishTurn;
    }

    private void FinishTurn()
    { 
        Turn++;
        Debug.Log($"Turn {Turn} finished.");
    }
}
