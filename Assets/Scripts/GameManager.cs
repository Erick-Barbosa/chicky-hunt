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

    private void Start() {
        //ShowMenu(true);
    }

    public void StartGame(int difficulty) {
        SceneManager.LoadScene(0);
        DifficultyNumber = difficulty;
        OnGameStart?.Invoke(difficulty);
        //ShowMenu(false);
    }
    public void HasGameOver(int points) {
        SaveHighScore(points);
        OnGameFinish?.Invoke(points);
        SceneManager.LoadScene(1);
        //ShowMenu(false);
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
        if (scene.buildIndex == 0) { // Substitua '1' pelo índice da sua cena de jogo no Build Settings
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
        if (points > PlayerHighScore) {
            Score data = new Score(points, DifficultyNumber);

            string directoryPath = Path.Combine(Application.persistentDataPath, data.difficulty);
            string filePath = Path.Combine(directoryPath, "savefile.json");

            // Garante que o diretório existe
            Directory.CreateDirectory(directoryPath);

            // Serializa e salva os dados
            string json = JsonUtility.ToJson(data);
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
