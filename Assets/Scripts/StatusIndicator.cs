using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {

    [SerializeField]
    private RectTransform healthBarRect;
    [SerializeField]
    private Text healthText;

    void Start() {
        if(healthBarRect == null) {
            Debug.LogError("STATUS INDICATOR: No health bar object referenced!");
        }
        if (healthText == null) {
            Debug.LogError("STATUS INDICATOR: No text object referenced!");
        }
    }

    public void SetHealth(int _currentHealth, int _maxHealth) {

        float _value = (float)_currentHealth / _maxHealth;

        healthBarRect.localScale = new Vector3(_value, healthBarRect.localScale.y, healthBarRect.localScale.z);
        healthText.text = _currentHealth + "/" + _maxHealth + " HP";
    }
}
