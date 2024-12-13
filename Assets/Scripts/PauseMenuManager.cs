using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour {
    [SerializeField] private GameObject menu;
    [SerializeField] private TextMeshProUGUI playerScore;
    [SerializeField] private Counter counter;
    [SerializeField] private Button backToMenu;
    [SerializeField] private Button restart;

    private void Awake() {
        HidePauseMenu();

        backToMenu.onClick.AddListener(() => {
            BackToMenu();
        });
        
        restart.onClick.AddListener(() => {
            RestartGame();
        });
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu() {
        GameManager.Instance.HasGameOver(counter.GetPoints());
    }

    public void ShowPauseMenu() {
        playerScore.text = counter.GetPoints().ToString();
        menu.SetActive(true);
        counter.gameObject.SetActive(false);
    }

    public void HidePauseMenu() {
        menu.SetActive(false);
        counter.gameObject.SetActive(true);
    }
}
