using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {
    [SerializeField] private List<Button> buttons = new List<Button>();
    [SerializeField] private List<GameObject> thingsToHideOnMenu;
    [SerializeField] private List<GameObject> menuItems;

    public event Action<int> OnGameStart;
    public event Action<int> OnGameFinish;

    public bool autoRestart;

    public int PlayerHighScore { get; private set; }

    public int DifficultyNumber { get; private set; } = 1;

    public void StartGame(int difficulty) {
        SceneManager.LoadScene(1);
        DifficultyNumber = difficulty;
        OnGameStart?.Invoke(difficulty);
    }
    public void HasGameOver(int points) {
        SaveHighScore(points);
        OnGameFinish?.Invoke(points);
        SceneManager.LoadScene(0);
    }

    private void ChangeIsActive(List<GameObject> children, bool state) {
        foreach (var child in children) { 
            child.SetActive(state);
        }
    }

    public void ShowMenu(bool shouldShow) {
        ChangeIsActive(menuItems, shouldShow);
        ChangeIsActive(thingsToHideOnMenu, !shouldShow);
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex == 1) { 
            OnGameStart?.Invoke(DifficultyNumber);
        }
    }

    public string GetDifficultyString(int value) {
        return value switch {
            1 => "Easy",
            2 => "Medium",
            4 => "Hard",
            _ => "Medium"
        };
    }

    [Serializable]
    class Score {
        public int value;
        public string difficulty;
    
        public Score(int value, int difficultyLevel) {
            this.value = value;
            difficulty = Instance.GetDifficultyString(difficultyLevel);
        }
    }

    public void SaveHighScore(int points) {
        string difficulty = GetDifficultyString(DifficultyNumber);
        string directoryPath = Path.Combine(Application.persistentDataPath, difficulty);
        string filePath = Path.Combine(directoryPath, "savefile.json");

        int currentHighScore = 0;

        if (File.Exists(filePath)) {
            try {
                string json = File.ReadAllText(filePath);
                Score data = JsonUtility.FromJson<Score>(json);
                if (data != null) {
                    currentHighScore = data.value;
                }
            }
            catch (Exception ex) {
                Debug.LogError($"Erro ao carregar high score: {ex.Message}");
            }
        }

        if (points > currentHighScore) {
            Score newHighScore = new Score(points, DifficultyNumber);

            Directory.CreateDirectory(directoryPath);

            string json = JsonUtility.ToJson(newHighScore);
            File.WriteAllText(filePath, json);
        }
    }


    public string LoadHighScore(string difficultyLevel) {
        string directoryPath = Path.Combine(Application.persistentDataPath, difficultyLevel);
        string filePath = Path.Combine(directoryPath, "savefile.json");

        if (File.Exists(filePath)) {
            try {
                string json = File.ReadAllText(filePath);
                Score data = JsonUtility.FromJson<Score>(json);

                if (data != null && data.difficulty != null && data.difficulty != null) {
                    return $"{data.difficulty} - {data.value}";
                }
                else {
                    return $"{difficultyLevel} - 0";
                }
            }
            catch (Exception ex) {
                Debug.Log(ex);
                return $"{difficultyLevel} - 0";
            }
        }
        else {
            Debug.LogWarning($"Arquivo de high score não encontrado para dificuldade: {difficultyLevel}");
            return $"{difficultyLevel} - 0";
        }
    }
}
