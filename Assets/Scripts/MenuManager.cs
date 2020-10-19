using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    [SerializeField]
    string hoverOverSound = "ButtonHover";
    [SerializeField]
    string pressSound = "ButtonPress";

    AudioManager audioManager;

    private void Start() {
        audioManager = AudioManager.instance;
        if(audioManager == null) {
            Debug.LogError("No audioManager found!");
        }
    }

    public void StartGame() {
        audioManager.playSound(pressSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        audioManager.playSound(pressSound);
        Debug.Log("QUIT GAME");
    }

    public void OnMouseOver() {
        audioManager.playSound(hoverOverSound);
    }
}
