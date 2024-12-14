using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChickyManager : MonoBehaviour {
    [SerializeField] private List<Transform> list = new List<Transform>();
    [SerializeField] private GameObject chicky;
    [SerializeField] private int timeBetweenSpawnsBase = 1;
    [SerializeField] private int maxPossiblePositions = 10; 
    [SerializeField] private float heighSpawnOffset = 0.2f;
    [SerializeField] private float minDistanceBetweenChickys = 4.0f;
    [SerializeField] private float delaySecondsOnStart = 2.0f;

    private float timeBetweenSpawns;

    [SerializeField] private Counter counter;
    [SerializeField] private PauseMenuManager menuManager;

    List<GameObject> chickiesObjects = new List<GameObject>();

    private bool hasStarted;

    private int chickiesAmount;

    private List<Vector3> recentSpawnPositions = new List<Vector3>(); 
    private float minXPosition;
    private float maxXPosition;

    [SerializeField] private int testDifficulty = 0;
    [SerializeField] private bool isOnTest;

    private void Awake() {
        counter = GameObject.Find("Counter").GetComponent<Counter>();

        if (counter == null) {
            throw new NullReferenceException("Counter has not been assigned in the Inspector.");
        }

        if (!isOnTest) {
            InitiateSpawner(GameManager.Instance.DifficultyNumber);
        } else {
            InitiateSpawner(testDifficulty);
        }
    }

    private Vector3 GetRandomPos(int randomIndex) {
        Renderer renderer = list[randomIndex].GetComponent<MeshRenderer>();
        Bounds bounds = renderer.bounds;

        minXPosition = bounds.min.x;
        maxXPosition = bounds.max.x;

        float randomXPos = UnityEngine.Random.Range(minXPosition, maxXPosition);
        return new Vector3(randomXPos, list[randomIndex].position.y + heighSpawnOffset, 0);
    }

    private bool IsPositionValid(Vector3 position) {
        foreach (Vector3 recentPos in recentSpawnPositions) {
            if (Vector3.Distance(position, recentPos) < minDistanceBetweenChickys) {
                return false;
            }
        }
        return true;
    }

    private void InitiateSpawner(int difficulty) {
        timeBetweenSpawns = Mathf.Max(0.2f, timeBetweenSpawnsBase / (float)difficulty);

        StartCoroutine(SpawnChicky());
    }

    private IEnumerator SpawnChicky() {
        if (!hasStarted) {
            yield return new WaitForSeconds(delaySecondsOnStart);
            hasStarted = true;
        }

        while (true) {
            while (menuManager.isOnMenu) {
                yield return new WaitForSeconds(1);
            }

            if (chickiesAmount >= 5) {
                ResetChickies();
                FinishMatch();
                yield break;
            }

            int randomIndex;
            Vector3 randomPos;

            do {
                randomIndex = UnityEngine.Random.Range(0, list.Count);
                randomPos = GetRandomPos(randomIndex);
            } while (!IsPositionValid(randomPos));

            chickiesObjects.Add(Instantiate(chicky, randomPos, chicky.transform.rotation));
            recentSpawnPositions.Add(randomPos);

            if (recentSpawnPositions.Count > maxPossiblePositions) {
                recentSpawnPositions.RemoveAt(0);
            }

            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    public void CountChickies(int valueToIncrease) {
        chickiesAmount += valueToIncrease;
    }

    private void ResetChickies() {
        hasStarted = false;

        foreach (var chicky in chickiesObjects) {
            Destroy(chicky.gameObject);
        }

        chickiesAmount = 0;
    }

    public void FinishMatch() {
        if (isOnTest) {
            menuManager.ShowPauseMenu();
            return;
        }

        if (GameManager.Instance.autoRestart) {
            GameManager.Instance.SaveHighScore(counter.GetPoints());
            SceneManager.LoadScene(1);
        }
        else {
            menuManager.isGameFinished = true;
            menuManager.ShowPauseMenu();
        }
    }
}
