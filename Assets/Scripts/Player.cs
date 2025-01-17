﻿using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {


    [System.Serializable]
    public class PlayerStats {
        public int maxHealth = 100;

        private int _currentHealth;
        public int currentHealth {
            get { return _currentHealth; }
            set { _currentHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init() {
            currentHealth = maxHealth;
        }
    }

    public PlayerStats stats = new PlayerStats();
    public int fallBoundry = -20;

    public string deathSoundName = "DeathVoice";
    public string damageSoundName = "Grunt";

    private AudioManager audioManager;

    [SerializeField]
    private StatusIndicator statusIndicator;

    void Start() {
        stats.Init();

        if(statusIndicator == null) {
            Debug.LogError("No status indicator referenced on player");
        } else {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }

        audioManager = AudioManager.instance;
        if (audioManager == null) {
            Debug.LogError("No audioManager found!");
        }
    }

    void Update() {
        if (transform.position.y <= fallBoundry) {
            DamagePlayer(100000000);
        }
    }

    public void DamagePlayer(int damage) {
        stats.currentHealth -= damage;
        if (stats.currentHealth <= 0) {
            Debug.Log("KILL PLAYER");
            audioManager.playSound(deathSoundName);
            GameMaster.KillPlayer(this);
        } else {
            audioManager.playSound(damageSoundName);
        }

        statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
    }
}
