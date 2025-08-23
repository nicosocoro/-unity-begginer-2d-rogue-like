using System;

public class ExitObject : CellObject
{
    public event Action OnPlayerEnteredEvent;

    public override void OnPlayerEntered()
    {
        OnPlayerEnteredEvent?.Invoke();
    }
}
