using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEDSceneManager : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;

    [SerializeField] private GameObject HomeScreen;
    [SerializeField] private GameObject GameUIScreen;
    [SerializeField] private GameObject Game3DOFScene;
    [SerializeField] private GameObject PauseScreen;

    [SerializeField] private GameObject laserPointer;

    [SerializeField] private AsteroidSpawner asteroidSpawner;

    public bool gamePlaying = false;
    public bool gamePaused = false;

    void Start()
    {
        Time.timeScale = 1; // Ensure game isn't frozen on load
        GoHome(); // Re-use GoHome logic to set initial state correctly
    }

    public void StartGame()
    {
        HomeScreen.SetActive(false);
        GameUIScreen.SetActive(true);
        Game3DOFScene.SetActive(true);
        PauseScreen.SetActive(false); // Ensure pause is off

        gamePlaying = true;
        gamePaused = false;
        laserPointer.SetActive(true);

        ClearAllUAPs();
        asteroidSpawner.StartSpawning();
        scoreManager.ResetVisuals();

        Time.timeScale = 1; // Unfreeze world
    }

    public void PauseGame()
    {
        if (gamePlaying && !gamePaused)
        {
            gamePaused = true;
            gamePlaying = false;

            laserPointer.SetActive(false);
            PauseScreen.SetActive(true);

            asteroidSpawner.StopSpawning();
            Time.timeScale = 0; // Freeze asteroids and physics
        }
    }

    public void ContinueGame()
    {
        if (gamePaused)
        {
            gamePaused = false;
            gamePlaying = true;

            laserPointer.SetActive(true);
            PauseScreen.SetActive(false);

            asteroidSpawner.StartSpawning();
            Time.timeScale = 1; // Resume asteroids and physics
        }
    }

    public void GoHome()
    {
        Time.timeScale = 1; // Reset time so menu isn't frozen

        HomeScreen.SetActive(true);
        GameUIScreen.SetActive(false);
        Game3DOFScene.SetActive(false);
        PauseScreen.SetActive(false);

        gamePaused = false;
        gamePlaying = false;
        laserPointer.SetActive(false);

        asteroidSpawner.StopSpawning();
        ClearAllUAPs();
    }

    public void ClearAllUAPs()
    {
        GameObject[] uaps = GameObject.FindGameObjectsWithTag("UAP");
        foreach (GameObject uap in uaps)
        {
            Destroy(uap);
        }
    }
}