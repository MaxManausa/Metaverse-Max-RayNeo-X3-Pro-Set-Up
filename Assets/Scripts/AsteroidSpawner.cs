using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("Drag all asteroid prefabs allowed for this level here")]
    [SerializeField] private GameObject[] asteroidPrefabs;
    [SerializeField] private Transform earthTarget;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Settings")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float minSpeed = 3f;
    [SerializeField] private float maxSpeed = 7f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnAsteroid), 1f, spawnInterval);
    }

    void SpawnAsteroid()
    {
        // Safety checks
        if (spawnPoints.Length == 0 || asteroidPrefabs.Length == 0) return;

        // 1. Pick a random prefab from your list (Gold, Rock, Ice, etc.)
        int randomPrefabIndex = Random.Range(0, asteroidPrefabs.Length);
        GameObject selectedPrefab = asteroidPrefabs[randomPrefabIndex];

        // 2. Pick a random spawn location
        int randomPointIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedSpot = spawnPoints[randomPointIndex];

        // 3. Create the asteroid
        GameObject newAsteroid = Instantiate(selectedPrefab, selectedSpot.position, Quaternion.identity);

        // 4. Set movement data
        AsteroidManager manager = newAsteroid.GetComponent<AsteroidManager>();
        if (manager != null)
        {
            manager.earthTarget = earthTarget;
            manager.speed = Random.Range(minSpeed, maxSpeed);
        }
    }
}