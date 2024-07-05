using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<GameObject> OnEnemyHit = null;
    public static event Action<GameObject> OnEnemyDie = null;

    public static void ReportOnEnemyHit(GameObject go)
    {
        OnEnemyHit?.Invoke(go);
    }

    public static void ReportOnEnemyDie(GameObject go)
    {
        OnEnemyDie?.Invoke(go);
    }
}
