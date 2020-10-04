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

    public float timeBetweenWaves = 5f;
    public float waveCountDown = 0f;
    private SpawnState state = SpawnState.COUNTING;

    void Start() {
        waveCountDown = timeBetweenWaves;

    }

    void Update() {
        if(waveCountDown <= 0) {
            if(state != SpawnState.SPAWNING) {
                // Needs to be used when calling an IEnumerator
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        } else {
            waveCountDown -= Time.deltaTime;
        }
    }


    // Allows waiting before returning
    IEnumerator SpawnWave(Wave _wave) {
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
    }

}
