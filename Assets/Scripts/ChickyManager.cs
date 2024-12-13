using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickyManager : MonoBehaviour {
    [SerializeField] private List<Transform> list = new List<Transform>();
    [SerializeField] private GameObject chicky;
    [SerializeField] private int timeBetweenSpawnsBase = 1;
    [SerializeField] private int maxPossiblePositions = 10; 
    [SerializeField] private float heighSpawnOffset = 0.2f;
    [SerializeField] private float minDistanceBetweenChickys = 4.0f;
    [SerializeField] private float delaySecondsOnStart = 2.0f;

    private float timeBetweenSpawns;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private Counter counter;

    List<GameObject> chickiesObjects = new List<GameObject>();

    private bool hasStarted;
    private bool hasGameOver;

    private int chickiesAmount;

    private List<Vector3> recentSpawnPositions = new List<Vector3>(); 
    private float minXPosition;
    private float maxXPosition;

    public event Action<int> OnGameFinish;

    private void OnEnable() {
        gameManager.OnGameStart += InitiateSpawner;
    }

    private void OnDisable() {
        gameManager.OnGameStart -= InitiateSpawner;
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
        hasGameOver = false;
        timeBetweenSpawns = Mathf.Max(0.2f, timeBetweenSpawnsBase / (float)difficulty);

        StartCoroutine(SpawnChicky());
    }
    private void StopSpawner(int number) {
        StopAllCoroutines();
    }

    private IEnumerator SpawnChicky() {
        if (!hasStarted) {
            yield return new WaitForSeconds(delaySecondsOnStart);
            hasStarted = true;
        }

        while (!hasGameOver) {
            if (chickiesAmount >= 5) {
                hasGameOver = true;
                OnGameFinish?.Invoke(counter.GetPoints());
                ResetChickies();
                StopSpawner(0);
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
        hasGameOver = true;

        foreach (var chicky in chickiesObjects) {
            Destroy(chicky.gameObject);
        }

        chickiesAmount = 0;

        counter.ResetPoints();
    }
}
