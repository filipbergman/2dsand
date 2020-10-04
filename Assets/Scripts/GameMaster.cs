using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    public static GameMaster gm;

    void Awake() {
        if (gm == null) {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2f;
    public Transform spawnPrefab;

    public CameraShake cameraShake;

    void Start() {
        if(cameraShake == null) {
            //Debug.LogError("No camera shake referenced in GM");
        }
    }


    public IEnumerator _RespawnPlayer()
    {
        GetComponent<AudioSource>().Play();
        
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
        Destroy(clone.gameObject, 3f);
        
    } 
    
    public static void KillPlayer(Player player) {
        Destroy(player.gameObject);
        gm.StartCoroutine(gm._RespawnPlayer());
    }
    
    public static void KillEnemy(Enemy enemy) {
        gm._killEnemy(enemy);
    }

    public void _killEnemy(Enemy _enemy) {
        GameObject _clone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity).gameObject;
        Destroy(_clone, 5f);
        cameraShake.Shake(_enemy.shakeAmount, _enemy.shakeLength);
        Destroy(_enemy.gameObject);
    }
    
}
