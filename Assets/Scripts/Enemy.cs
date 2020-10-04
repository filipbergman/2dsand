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
        
        public int damage = 40;

        public void Init() {
            currentHealth = maxHealth;
        }
    }

    public EnemyStats stats = new EnemyStats();
    public Transform deathParticles;
    public float shakeAmount = 0.1f;
    public float shakeLength = 0.1f;

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    private void Start() {
        stats.Init();

        if(statusIndicator != null) {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }

        if(deathParticles == null)
        {
            Debug.LogError("No death particles referenced!");
        }
    }

    public void DamageEnemy(int damage) {
        stats.currentHealth -= damage;

        if (stats.currentHealth <= 0) {
            Debug.Log("KILL ENEMY");
            GameMaster.KillEnemy(this);
        }

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }
    }

    // Is called every time this object collides with something
    void OnCollisionEnter2D(Collision2D _colInfo) {
        Player _player = _colInfo.collider.GetComponent<Player>();
        if(_player != null) {
            _player.DamagePlayer(stats.damage);

            DamageEnemy(9999999);
        }


    }


}
