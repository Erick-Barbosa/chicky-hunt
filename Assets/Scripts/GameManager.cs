using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [SerializeField] private List<Button> buttons = new List<Button>();
    [SerializeField] private List<GameObject> thingsToHideOnMenu;
    [SerializeField] private List<GameObject> menuItems;
    [SerializeField] private ChickyManager chickyManager;

    public event Action<int> OnGameStart;

    private void OnEnable() {
        chickyManager.OnGameFinish += EndGame;
    }
    private void OnDisable() {
        chickyManager.OnGameFinish -= EndGame;
    }

    private void Start() {
        ShowMenu(true);
    }

    public void StartGame(int difficulty) {
        OnGameStart?.Invoke(difficulty);

        ShowMenu(false);
    }

    public void EndGame(int score) {
        ShowMenu(true);
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
