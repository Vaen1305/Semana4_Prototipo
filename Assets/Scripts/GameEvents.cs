using System;

public static class GameEvents
{
    public static event Action OnPlayerWin;
    public static event Action OnPlayerLoss;

    public static void TriggerWin()
    {
        OnPlayerWin?.Invoke();
    }

    public static void TriggerLoss()
    {
        OnPlayerLoss?.Invoke();
    }
}
