using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

    public enum SpawnState { SPAWNING, WAITING, COUNTING };


    [System.Serializable]
    public class Wave {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;
    public int NextWave {
        get { return nextWave + 1; }
    }

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 3f;
    private float wavecountdown = 0f;
    public float Wavecountdown {
        get { return wavecountdown + 1; }
    }

    private SpawnState state = SpawnState.COUNTING;
    public SpawnState State {
        get { return state; }
    }

    private float searchcountdown = 1f;

    void Start() {
        if (spawnPoints.Length == 0) {
            Debug.LogError("No spawnpoints referenced!");
        }
        wavecountdown = timeBetweenWaves;
    }

    void Update() {
        if(state == SpawnState.WAITING) {
            if(!EnemyIsAlive()) {
                WaveCompleted();
                return;
            } else {
                return; // dont check anything else if enemies are still alive
            }
        }

        if(wavecountdown <= 0) {
            if(state != SpawnState.SPAWNING) {
                // Needs to be used when calling an IEnumerator
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        } else {
            wavecountdown -= Time.deltaTime;
        }
    }

    bool EnemyIsAlive(){
        searchcountdown -= Time.deltaTime;
        if(searchcountdown <= 0) {
            searchcountdown = 1f;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0) {
                return false;
            }
        }
        return true;
    }

    void WaveCompleted() {
        Debug.Log("Wave completed");
        state = SpawnState.COUNTING;
        wavecountdown = timeBetweenWaves;

        if(nextWave + 1 > waves.Length -1) {
            nextWave = 0;
            Debug.Log("Completed all waves1! Looping...");
        } else {
            nextWave++;
        }
        
    }

    // Allows waiting before returning
    IEnumerator SpawnWave(Wave _wave) {
        Debug.Log("Spawning wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        for(int i = 0; i < _wave.count; i++) {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f /_wave.rate);
        }

        state = SpawnState.WAITING;


        yield break;
    }

    void SpawnEnemy(Transform _enemy) {
        Debug.Log("Spawning enemy: " + _enemy.name);
        
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }

}
