using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class Sound {

    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 0.1f;

    [Range(0f, 0.5f)]
    public float volumeRand = 0.1f;
    [Range(0f, 1f)]
    public float pitchRand = 0.1f;

    public bool loop = false;

    private AudioSource source;


    public void setSource(AudioSource _source) {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play() {
        source.volume = volume * (1 + Random.Range(-volumeRand/2f, volumeRand/2f));
        source.pitch = pitch * (1 + Random.Range(-pitchRand / 2f, pitchRand / 2f));
        source.Play();
    }

    public void Stop() {
        source.Stop();
    }

}

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    
    [SerializeField]
    Sound[] sounds;

    private void Awake() {
        if(instance != null) {
            if(instance != this) {
                Destroy(gameObject);
            }
        } else {
            // If we change scene this will keep the object that we previously had
            instance = this;
            DontDestroyOnLoad(this);
        }

    }

    void Start() {
        for(int i = 0; i < sounds.Length; i++) {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].setSource(_go.AddComponent<AudioSource>());
        }

        playSound("Music");
    }

    //private void Update() {
    //    if(Time.time > 5f) {
    //        stopSound("Music");
    //    }
    //}

    public void playSound(string _name) {
        for(int i = 0; i < sounds.Length; i++) {
            if(sounds[i].name == _name) {
                sounds[i].Play();
                return;
            }
        }

        // NO SOUND WITH _name
        Debug.Log("AudioManager: sound not found in list: " + _name);
    }

    public void stopSound(string _name) {
        for (int i = 0; i < sounds.Length; i++) {
            if (sounds[i].name == _name) {
                sounds[i].Stop();
                return;
            }
        }

        // NO SOUND WITH _name
        Debug.Log("AudioManager: sound not found in list: " + _name);
    }
}
