using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    public static GameMaster gm;

    private void Start() {
        if (gm == null) {
            gm = this;
        }
    }
    public Transform playerPrefab;
    public Transform spawnPoint;
    
    public void RespawnPlayer() {
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("TODO: add spawn particles");
    } 
    
    public static void KillPlayer(Player player) {
        Destroy(player.gameObject);
        gm.RespawnPlayer();
    }
    
    
    
}
