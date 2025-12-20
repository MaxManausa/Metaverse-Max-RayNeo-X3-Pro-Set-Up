using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject[] asteroidPrefabs;
    [SerializeField] private Transform earthTarget;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Settings")]
    [SerializeField] private float minSpeed = 3f;
    [SerializeField] private float maxSpeed = 7f;

    // The list of possible spawn delays you requested
    private float[] possibleIntervals = { 0.5f, 0.75f, 1f, 1.5f, 2f, 3f, 4f };
    private Coroutine spawnCoroutine;

    public void StartSpawning()
    {
        StopSpawning();
        spawnCoroutine = StartCoroutine(SpawnLoop());
        Debug.Log("Dynamic Asteroid Spawning Started");
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
        Debug.Log("Asteroid Spawning Stopped");
    }

    private IEnumerator SpawnLoop()
    {
        // Initial wait before first spawn
        yield return new WaitForSeconds(1f);

        while (true)
        {
            SpawnAsteroid();

            // Pick a random interval from your list
            float nextDelay = possibleIntervals[Random.Range(0, possibleIntervals.Length)];

            yield return new WaitForSeconds(nextDelay);
        }
    }

    void SpawnAsteroid()
    {
        if (spawnPoints.Length == 0 || asteroidPrefabs.Length == 0) return;

        int randomPrefabIndex = Random.Range(0, asteroidPrefabs.Length);
        GameObject selectedPrefab = asteroidPrefabs[randomPrefabIndex];

        int randomPointIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedSpot = spawnPoints[randomPointIndex];

        GameObject newAsteroid = Instantiate(selectedPrefab, selectedSpot.position, Quaternion.identity);

        AsteroidManager manager = newAsteroid.GetComponent<AsteroidManager>();
        if (manager != null)
        {
            manager.earthTarget = earthTarget;
            manager.speed = Random.Range(minSpeed, maxSpeed);
        }
    }
}