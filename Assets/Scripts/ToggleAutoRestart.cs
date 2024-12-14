using UnityEngine;
using UnityEngine.UI;

public class ToggleAutoRestart : MonoBehaviour {
    private Toggle toggle;

    private void Awake() {
        toggle = GetComponent<Toggle>();

        toggle.onValueChanged.AddListener((value) => {
            GameManager.Instance.autoRestart = value;
        });
    }
}
