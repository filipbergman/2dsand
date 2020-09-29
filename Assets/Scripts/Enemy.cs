using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public Transform explotionPrefab;

    [Serializable]
    public class EnemyStats {
        public int maxHealth = 100;
        
        private int _currentHealth;
        public int currentHealth {
            get { return _currentHealth; }
            set { _currentHealth = Mathf.Clamp(value, 0, maxHealth);} // minimum is 0, maximum is maxhealth
        }

        public void Init() {
            currentHealth = maxHealth;
        }
    }

    public EnemyStats stats = new EnemyStats();

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    private void Start() {
        stats.Init();

        if(statusIndicator != null) {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }
    }

    public void DamageEnemy(int damage) {
        stats.currentHealth -= damage;

        if (stats.currentHealth <= 0) {
            Debug.Log("KILL ENEMY");
            GameMaster.KillEnemy(this);
            Transform explotion = Instantiate(explotionPrefab, transform.position, Quaternion.identity) as Transform;
            Destroy(explotion.gameObject, 1.0f);
        }

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }
    }
}
