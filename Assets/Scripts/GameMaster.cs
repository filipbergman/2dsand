using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{

    public static GameMaster gm;

    private static int _remainingLives = 1;
    public static int RemainingLives {
        get { return _remainingLives; }
    }

    private static int _score;
    public static int Score {
        get { return _score; }
    }

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

    [SerializeField]
    private GameObject gameOverUI;

    void Start() {
        _remainingLives = 1;
        _score = 0;
        if (cameraShake == null) {
            //Debug.LogError("No camera shake referenced in GM");
        }
    }

    public void EndGame() {
        Debug.Log("GAME OVER");
        gameOverUI.SetActive(true);
    }

    public IEnumerator _RespawnPlayer() {
        GetComponent<AudioSource>().Play();
        
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
        Destroy(clone.gameObject, 3f);
        
    } 
    
    public static void KillPlayer(Player player) {
        Destroy(player.gameObject);
        _remainingLives--;
        if(_remainingLives <= 0) {
            gm.EndGame();
        } else {
            gm.StartCoroutine(gm._RespawnPlayer());
        }
    }
    
    public static void KillEnemy(Enemy enemy) {
        gm._killEnemy(enemy);
        
        _score += 10;
    }

    public void _killEnemy(Enemy _enemy) {
        GameObject _clone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity).gameObject;
        Destroy(_clone, 5f);
        cameraShake.Shake(_enemy.shakeAmount, _enemy.shakeLength);
        Destroy(_enemy.gameObject);
    }
    
}
