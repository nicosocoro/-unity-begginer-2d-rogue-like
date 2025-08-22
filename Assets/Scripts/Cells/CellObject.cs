using UnityEngine;

public abstract class CellObject : MonoBehaviour
{
    public virtual void OnInit(Vector2Int position) { }
    public virtual void OnPlayerEntered() { }
}
