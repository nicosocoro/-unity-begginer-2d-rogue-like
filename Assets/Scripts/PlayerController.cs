using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private BoardManager _board;
    private Vector2Int _currentPosition;
    private Vector2Int? _targetPosition = null;
    private Animator _animator;
    private bool _isMoving = false;

    public event Action<Vector2Int> OnPlayerMoved;
    public event Action OnRestartRequested;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        _board = boardManager;
        _currentPosition = cell;
        _isMoving = false;
        _targetPosition = null;
        _animator.SetBool("Moving", false);

        transform.position = _board.GetCellPosition(_currentPosition);
    }

    public void Update()
    {
        if (GameManager.Instance.IsGameActive)
        {
            HandleIsMoving();
            HandleInputInActiveGame();
        }
        else
        {
            HandleInputInInactiveGame();
        }
    }

    private void HandleInputInActiveGame()
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

    private void HandleInputInInactiveGame()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            OnRestartRequested?.Invoke();
        }
    }

    private void TryMoveTo(Vector2Int targetCell)
    {
        if (!_isMoving && _board.IsCellAvailableForPlayer(targetCell))
        {
            MoveTo(targetCell);
            OnPlayerMoved?.Invoke(targetCell);
        }
    }

    private void MoveTo(Vector2Int targetCell)
    {
        _targetPosition = targetCell;
        _isMoving = true;
        _animator.SetBool("Moving", true);

        _currentPosition = targetCell;
    }

    private void HandleIsMoving()
    {
        if (_isMoving && _targetPosition != null)
        {
            Vector3 targetWorldPos = _board.GetCellPosition(_targetPosition.Value);
            if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
            {
                transform.position = targetWorldPos;
                _isMoving = false;
                _targetPosition = null;
                _animator.SetBool("Moving", false);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, Time.deltaTime * 5f);
            }
        }
    }
}