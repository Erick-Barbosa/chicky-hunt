using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {
    [SerializeField] private List<Button> buttons = new List<Button>();
    [SerializeField] private List<GameObject> thingsToHideOnMenu;
    [SerializeField] private List<GameObject> menuItems;

    public event Action<int> OnGameStart;
    public event Action<int> OnGameFinish;

    static public int Difficulty {  get; private set; }

    private void Start() {
        //ShowMenu(true);
    }

    public void StartGame(int difficulty) {
        SceneManager.LoadScene(0);
        Difficulty = difficulty;
        OnGameStart?.Invoke(difficulty);
        //ShowMenu(false);
    }
    public void HasGameOver(int points) {
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
}
