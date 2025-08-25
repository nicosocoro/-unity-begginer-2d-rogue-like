using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int _level = 1;

    public int Level { get => _level; private set { _level = value; } }

    public void IncreaseLevel()
    {
        _level++;
    }

    public void Restart()
    {
        _level = 1;
    }
}