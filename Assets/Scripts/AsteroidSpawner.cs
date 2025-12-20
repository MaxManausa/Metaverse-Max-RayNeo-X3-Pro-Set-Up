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
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float minSpeed = 3f;
    [SerializeField] private float maxSpeed = 7f;

    // --- Control Methods ---

    /// <summary>
    /// Starts the spawning loop.
    /// </summary>
    public void StartSpawning()
    {
        // First, stop any existing loop to prevent "double spawning"
        StopSpawning();

        InvokeRepeating(nameof(SpawnAsteroid), 1f, spawnInterval);
        Debug.Log("Asteroid Spawning Started");
    }

    /// <summary>
    /// Stops the spawning loop.
    /// </summary>
    public void StopSpawning()
    {
        CancelInvoke(nameof(SpawnAsteroid));
        Debug.Log("Asteroid Spawning Stopped");
    }

    // --- Internal Logic ---

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