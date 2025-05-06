using System;

public static class PointsSystem
{
    public static event Action<int> OnPointsChanged;

    public static void UpdatePoints(int puntos)
    {
        OnPointsChanged?.Invoke(puntos);
    }
}
