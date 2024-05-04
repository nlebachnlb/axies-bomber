using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillTracker : MonoBehaviour
{
    public int killedEnemies = 0;

    private void OnEnable()
    {
        EventBus.onEnemyDeath += OnEnemyDeath;
    }

    private void OnDisable()
    {
        EventBus.onEnemyDeath -= OnEnemyDeath;
    }

    private void OnEnemyDeath()
    {
        killedEnemies += 1;
    }
}
