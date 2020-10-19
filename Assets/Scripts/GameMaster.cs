using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour { 
    public static GameMaster gm;

    [SerializeField]
    private int maxLives = 1;
    private static int _remainingLives;
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
    public string respawnCountDownName = "RespawnCountDown";
    public string spawnSoundName = "Spawn";

    public CameraShake cameraShake;

    [SerializeField]
    private GameObject gameOverUI;

    public string gameOverSoundName = "GameOver";

    // cache
    private AudioManager audioManager;

    void Start() {
        _remainingLives = maxLives;
        _score = 0;
        if (cameraShake == null) {
            //Debug.LogError("No camera shake referenced in GM");
        }

        // caching
        audioManager = AudioManager.instance;
        if(audioManager == null) {
            Debug.LogError("FREAK OUT! NO AudioManager found in this scene");
        }
    }

    public void EndGame() {
        audioManager.playSound(gameOverSoundName);
        Debug.Log("GAME OVER");
        gameOverUI.SetActive(true);
    }

    public IEnumerator _RespawnPlayer() {
        audioManager.playSound(respawnCountDownName);
        
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
        Destroy(clone.gameObject, 3f);
        audioManager.playSound(spawnSoundName);
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
        // Sound
        audioManager.playSound(_enemy.deathSoundName);
        // Add particles
        GameObject _clone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity).gameObject;
        Destroy(_clone, 5f);

        // Shake camera
        cameraShake.Shake(_enemy.shakeAmount, _enemy.shakeLength);
        Destroy(_enemy.gameObject);
    }
    
}
