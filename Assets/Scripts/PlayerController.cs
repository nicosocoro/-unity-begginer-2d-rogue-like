using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager m_Board;
    private Vector2Int m_CellPosition;

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        m_CellPosition = cell;

        transform.position = m_Board.GetCellPosition(m_CellPosition);
    }

    public void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        Vector2Int targetCell = m_CellPosition;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            targetCell = m_CellPosition + Vector2Int.up;
        }
        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            targetCell = m_CellPosition + Vector2Int.down;
        }
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            targetCell = m_CellPosition + Vector2Int.left;
        }
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            targetCell = m_CellPosition + Vector2Int.right;
        }

        if (targetCell != m_CellPosition)
        {
            TryMoveTo(targetCell);
        }
    }

    private void TryMoveTo(Vector2Int targetCell)
    {
        if (m_Board.IsCellAvailableForPlayer(targetCell))
        {
            MoveTo(targetCell);
        }
    }

    private void MoveTo(Vector2Int targetCell)
    {
        m_CellPosition = targetCell;
        transform.position = m_Board.GetCellPosition(m_CellPosition);
    }
}