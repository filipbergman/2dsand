using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [Serializable]
    public class PlayerStats {
        public int Health = 100;
    
    }

    public PlayerStats playerStats = new PlayerStats();
    public int fallBoundry = -20;

    private void Update() {
        if (transform.position.y <= fallBoundry) {
            DamagePlayer(100000000);
        }
    }

    public void DamagePlayer(int damage) {
        playerStats.Health -= damage;
        if (playerStats.Health <= 0) {
            Debug.Log("KILL PLAYER");
            GameMaster.KillPlayer(this);
        }
    }
    
    
    
}
