using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

    [SerializeField]
    string mouseHoverSound = "ButtonHover";

    [SerializeField]
    string buttonPressSound = "ButtonPress";

    private AudioManager audioManager;

    private void Start() {

        audioManager = AudioManager.instance;
        if (audioManager == null) {
            Debug.LogError("No audioManager found!");
        }
    }

    public void Quit() {
        audioManager.playSound(buttonPressSound);
        Debug.Log("APPLICATION QUIT!");
        Application.Quit();
    }

    public void Retry() {
        audioManager.playSound(buttonPressSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMouseOver() {
        audioManager.playSound(mouseHoverSound);
    }

}
