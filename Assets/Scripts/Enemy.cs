using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Serializable]
    public class EnemyStats
    {
        public int Health = 100;

    }

    public EnemyStats stats = new EnemyStats();

    public void DamageEnemy(int damage)
    {
        stats.Health -= damage;
        if (stats.Health <= 0)
        {
            Debug.Log("KILL ENEMY");
            GameMaster.KillEnemy(this);
        }
    }
}
