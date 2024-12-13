using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
    [SerializeField] private Button easy;
    [SerializeField] private Button medium;
    [SerializeField] private Button hard;
    [SerializeField] private TextMeshProUGUI highScoreEasy;
    [SerializeField] private TextMeshProUGUI highScoreMedium;
    [SerializeField] private TextMeshProUGUI highScoreHard;

    private Dictionary<string, string> highScores = new Dictionary<string, string>();

    private void Awake() {
        easy.onClick.AddListener(() => {
            GameManager.Instance.StartGame(easy.GetComponent<Difficulty>().value);
        });
        medium.onClick.AddListener(() => {
            GameManager.Instance.StartGame(medium.GetComponent<Difficulty>().value);
        });
        hard.onClick.AddListener(() => {
            GameManager.Instance.StartGame(hard.GetComponent<Difficulty>().value);
        });

        highScores["Easy"] = GameManager.Instance.LoadHighScore("Easy");
        highScores["Medium"] = GameManager.Instance.LoadHighScore("Medium");
        highScores["Hard"] = GameManager.Instance.LoadHighScore("Hard");

        highScoreEasy.text = highScores["Easy"];
        highScoreMedium.text = highScores["Medium"];
        highScoreHard.text = highScores["Hard"];
    }
}
