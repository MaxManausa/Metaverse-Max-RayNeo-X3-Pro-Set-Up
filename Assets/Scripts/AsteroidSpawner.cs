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
    private bool _uapPhaseTriggered = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartSpawning()
    {
        StopSpawning();
        levelCoroutine = StartCoroutine(PlayLevelSequence());
    }

    public void StopSpawning()
    {
        if (levelCoroutine != null) StopCoroutine(levelCoroutine);
        _uapPhaseTriggered = false;
    }

    private IEnumerator PlayLevelSequence()
    {
        var config = LevelManager.Instance.currentLevel;
        yield return new WaitForSeconds(2f);

        int asteroidsToHitTarget = Mathf.CeilToInt(config.asteroidCount * 0.6f);
        while (ScoreManager.Instance.targetsDestroyed < asteroidsToHitTarget)
        {
            SpawnIndividual(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count)], AsteroidManager.EnemyType.Asteroid, 1f);
            yield return new WaitForSeconds(GetRandomInterval());
        }

        if (config.alienCount > 0 || config.warriorAlienCount > 0 || config.bossAlienCount > 0)
        {
            if (uapWarningText != null)
            {
                uapWarningText.text = "UAP INBOUND";
                uapWarningText.gameObject.SetActive(true);
                yield return new WaitForSeconds(5f);
                uapWarningText.gameObject.SetActive(false);
            }

            int aCount = config.alienCount, wCount = config.warriorAlienCount, bCount = config.bossAlienCount;
            int rAst = config.asteroidCount - ScoreManager.Instance.targetsDestroyed;

            while (aCount > 0 || wCount > 0 || bCount > 0 || rAst > 0)
            {
                if (bCount > 0) { SpawnIndividual(bossPrefab, AsteroidManager.EnemyType.Boss, 0.6f); bCount--; }
                else if (wCount > 0) { SpawnIndividual(warriorPrefab, AsteroidManager.EnemyType.Warrior, 4f); wCount--; }
                else if (aCount > 0) { SpawnIndividual(alienPrefab, AsteroidManager.EnemyType.Alien, 1.4f); aCount--; }
                else if (rAst > 0) { SpawnIndividual(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count)], AsteroidManager.EnemyType.Asteroid, 1f); rAst--; }
                yield return new WaitForSeconds(GetRandomInterval());
            }
        }

        while (GameObject.FindObjectsByType<AsteroidManager>(FindObjectsSortMode.None).Length > 0) yield return new WaitForSeconds(0.5f);

        if (ScoreManager.Instance.targetsDestroyed >= ScoreManager.Instance.targetsToWin) ScoreManager.Instance.EndLevel(true);
        else StartCoroutine(PlayLevelSequence());
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
            am.earthTarget = LevelManager.Instance.currentTargetTransform;
            if (type == AsteroidManager.EnemyType.Warrior && warriorStopPoints.Count > 0)
                am.warriorStopPoint = warriorStopPoints[Random.Range(0, warriorStopPoints.Count)];
        }
    }

    private float GetRandomInterval() => possibleIntervals[Random.Range(0, possibleIntervals.Length)];
}