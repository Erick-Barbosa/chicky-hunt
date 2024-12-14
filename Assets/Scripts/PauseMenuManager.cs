using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour {
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject gun;
    [SerializeField] private TextMeshProUGUI playerScore;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Counter counter;
    [SerializeField] private Button backToMenu;
    [SerializeField] private Button restart;

    [SerializeField] private int baseResumeTime = 3;
    private int resumeTime;

    public bool isOnMenu;

    private void Awake() {
        HidePauseMenu(false);
        timeText.enabled = false;

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
    public IEnumerator ResumeGame() {
        if (resumeTime > 0) {
            timeText.text = resumeTime.ToString();

            resumeTime--;

            yield return new WaitForSeconds(1);

            StartCoroutine(ResumeGame());
            yield break;
        } 

        isOnMenu = false;
        gun.SetActive(true);
        timeText.enabled = false;
        yield return null;
    }

    public void BackToMenu() {
        GameManager.Instance.HasGameOver(counter.GetPoints());
    }

    public void ShowPauseMenu() {
        resumeTime = baseResumeTime;

        timeText.text = resumeTime.ToString();
        playerScore.text = counter.GetPoints().ToString();

        menu.SetActive(true);
        gun.SetActive(false);
        counter.gameObject.SetActive(false);
        timeText.enabled = false;

        isOnMenu = true;
    }

    public void HidePauseMenu(bool shouldShowTimer) {
        menu.SetActive(false);
        counter.gameObject.SetActive(true);
       
        if (!shouldShowTimer) {
            isOnMenu = false;
            gun.SetActive(true);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!isOnMenu) {
                ShowPauseMenu();
            }
            else {
                timeText.enabled = true;
                HidePauseMenu(true);
                StartCoroutine(ResumeGame());
            }
        }
    }
}
