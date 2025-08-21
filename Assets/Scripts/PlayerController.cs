using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager _board;
    private Vector2Int _currentPosition;

    public event Action<Vector2Int> OnPlayerMoved;

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        _board = boardManager;
        _currentPosition = cell;

        transform.position = _board.GetCellPosition(_currentPosition);
    }

    public void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        Vector2Int targetCell = _currentPosition;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            targetCell = _currentPosition + Vector2Int.up;
        }
        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            targetCell = _currentPosition + Vector2Int.down;
        }
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            targetCell = _currentPosition + Vector2Int.left;
        }
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            targetCell = _currentPosition + Vector2Int.right;
        }

        if (targetCell != _currentPosition)
        {
            TryMoveTo(targetCell);
        }
    }

    private void TryMoveTo(Vector2Int targetCell)
    {
        if (_board.IsCellAvailableForPlayer(targetCell))
        {
            MoveTo(targetCell);
            OnPlayerMoved?.Invoke(targetCell);
        }
    }

    private void MoveTo(Vector2Int targetCell)
    {
        _currentPosition = targetCell;
        transform.position = _board.GetCellPosition(_currentPosition);
    }
}