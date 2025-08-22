using UnityEngine;

public abstract class CellObject : MonoBehaviour
{
    private Vector2Int? _position;

    public virtual void OnInit(Vector2Int position)
    {
        _position = position;
    }

    public virtual bool OnPlayerTryingToEnter() => true;
    public virtual void OnPlayerEntered() { }
    protected void OnDestroy()
    {
        if (_position != null)
        {
            GameManager.Instance.BoardManager.OnFreeCell(_position.Value);
        }
        Destroy(gameObject);
    }
}
