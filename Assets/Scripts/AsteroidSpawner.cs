using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public static AsteroidSpawner Instance;

    [Header("Enemy Prefabs")]
    [SerializeField] private List<GameObject> asteroidPrefabs;
    [SerializeField] private GameObject alienPrefab;
    [SerializeField] private GameObject warriorPrefab;
    [SerializeField] private GameObject bossPrefab;

    [Header("UI Feedback")]
    [SerializeField] private TextMeshProUGUI uapWarningText;

    [Header("Setup")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private List<Transform> warriorStopPoints;

    [Header("Settings")]
    [SerializeField] private float minSpeed = 3f;
    [SerializeField] private float maxSpeed = 7f;

    private float[] possibleIntervals = { 0.5f, 0.75f, 1f, 1.5f };
    private Coroutine levelCoroutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartSpawning()
    {
        StopSpawning();

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.targetsDestroyed = 0;
        }

        levelCoroutine = StartCoroutine(PlayLevelSequence());
    }

    public void StopSpawning()
    {
        if (levelCoroutine != null) StopCoroutine(levelCoroutine);
        if (uapWarningText != null) uapWarningText.gameObject.SetActive(false);
    }

    private IEnumerator PlayLevelSequence()
    {
        if (LevelManager.Instance == null || LevelManager.Instance.currentLevel.levelNumber == 0)
            yield break;

        var config = LevelManager.Instance.currentLevel;
        yield return new WaitForSeconds(2f);

        // --- PHASE 1: NON-STOP ASTEROIDS ---
        // Keeps spawning until you have successfully destroyed 60% of the level goal
        int phase1Target = Mathf.CeilToInt(config.asteroidCount * 0.6f);

        while (ScoreManager.Instance.targetsDestroyed < phase1Target)
        {
            SpawnIndividual(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count)], AsteroidManager.EnemyType.Asteroid, 1f);
            yield return new WaitForSeconds(GetRandomInterval());
        }

        // --- PHASE 2: UAP & FINAL TARGETS ---
        // Triggered once you pass the 60% mark
        if (config.alienCount > 0 || config.warriorAlienCount > 0 || config.bossAlienCount > 0)
        {
            if (uapWarningText != null)
            {
                uapWarningText.text = "[UAP INBOUND]";
                uapWarningText.gameObject.SetActive(true);
                yield return new WaitForSeconds(3f);
                uapWarningText.gameObject.SetActive(false);
            }
        }

        // Keep spawning until the total level target is met
        // This includes a mix of aliens and asteroids
        while (ScoreManager.Instance.targetsDestroyed < ScoreManager.Instance.targetsToWin)
        {
            // Logic to prioritize spawning special enemies first if they are defined in config
            if (config.bossAlienCount > 0 && GameObject.FindObjectsByType<AsteroidManager>(FindObjectsSortMode.None).Length < 5)
            {
                SpawnIndividual(bossPrefab, AsteroidManager.EnemyType.Boss, 0.6f);
            }
            else if (config.warriorAlienCount > 0 && Random.value > 0.7f)
            {
                SpawnIndividual(warriorPrefab, AsteroidManager.EnemyType.Warrior, 4f);
            }
            else if (config.alienCount > 0 && Random.value > 0.5f)
            {
                SpawnIndividual(alienPrefab, AsteroidManager.EnemyType.Alien, 1.4f);
            }
            else
            {
                // Fill the rest of the time with standard asteroids
                SpawnIndividual(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count)], AsteroidManager.EnemyType.Asteroid, 1f);
            }

            yield return new WaitForSeconds(GetRandomInterval());
        }

        // --- END OF LEVEL ---
        // Spawning has stopped because target was hit. 
        // Now wait for the field to clear of leftovers.
        while (GameObject.FindObjectsByType<AsteroidManager>(FindObjectsSortMode.None).Length > 0)
        {
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1.5f);

        // Final victory check
        if (ScoreManager.Instance.targetsDestroyed >= ScoreManager.Instance.targetsToWin)
        {
            ScoreManager.Instance.EndLevel(true);
        }
    }

    private void SpawnIndividual(GameObject prefab, AsteroidManager.EnemyType type, float speedMult)
    {
        if (prefab == null || spawnPoints.Length == 0) return;

        Transform spot = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(prefab, spot.position, Quaternion.identity);

        AsteroidManager am = enemy.GetComponent<AsteroidManager>();
        if (am != null)
        {
            am.type = type;
            am.speed = Random.Range(minSpeed, maxSpeed) * speedMult;
            if (LevelManager.Instance != null)
                am.earthTarget = LevelManager.Instance.currentTargetTransform;

            if (type == AsteroidManager.EnemyType.Warrior && warriorStopPoints.Count > 0)
                am.warriorStopPoint = warriorStopPoints[Random.Range(0, warriorStopPoints.Count)];
        }
    }

    private float GetRandomInterval() => possibleIntervals[Random.Range(0, possibleIntervals.Length)];
}