using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour {
    public TextMeshProUGUI CounterText;

    private int count = 0;

    private void Start() {
        count = 0;
    }

    public void UpdatePoints() {
        count += 1;
        CounterText.text = count.ToString();
    }

    public int GetPoints() => count;

    public void ResetPoints() { 
        count = 0;
        CounterText.text = count.ToString();
    }
}
