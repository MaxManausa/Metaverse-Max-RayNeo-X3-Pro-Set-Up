using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ******************************************************************************************
 * HUGE NOTE: THIS IS A JOKE SCRIPT THAT WILL FULL FUCK UP THE VIEW OF THE EARTH 
 * AND COULD MAYBE BE USED AS PSYCHOLOGICAL WARFARE FROM ALIENS.
 * ******************************************************************************************
 */

public class SEDSceneManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private ResetHeadTrack resetHeadTrack;
    [SerializeField] private AsteroidSpawner asteroidSpawner;

    [Header("UI Screens")]
    [SerializeField] private GameObject HomeScreen;
    [SerializeField] private GameObject GameUIScreen;
    [SerializeField] private GameObject Game3DOFScene;
    [SerializeField] private GameObject PauseScreen;
    [SerializeField] private GameObject FailScreen;
    [SerializeField] private GameObject WonScreen;

    [Header("Tools")]
    [SerializeField] private GameObject laserPointer;

    private GameObject[] allScreens;

    [HideInInspector] public bool gamePlaying = false;
    [HideInInspector] public bool gamePaused = false;

    void Awake()
    {
        // Cache all screens into an array for high-speed UI switching
        allScreens = new GameObject[] { HomeScreen, GameUIScreen, PauseScreen, FailScreen, WonScreen };
    }

    void Start()
    {
        Time.timeScale = 1;
        GoHome();
    }

    public void StartGame()
    {
        ToggleUI(GameUIScreen); // Automatically hides all others including Fail/Won

        Game3DOFScene.SetActive(true);
        resetHeadTrack.OnReset();

        gamePlaying = true;
        gamePaused = false;
        laserPointer.SetActive(true);

        ClearAllUAPs();
        asteroidSpawner.StartSpawning();
        scoreManager.ResetVisuals();

        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        if (!gamePlaying || gamePaused) return;

        gamePaused = true;
        gamePlaying = false;

        ToggleUI(PauseScreen);
        laserPointer.SetActive(false);

        asteroidSpawner.StopSpawning();
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        if (!gamePaused) return;

        gamePaused = false;
        gamePlaying = true;

        ToggleUI(GameUIScreen);
        laserPointer.SetActive(true);

        asteroidSpawner.StartSpawning();
        Time.timeScale = 1;
    }

    public void FailGame()
    {
        Game3DOFScene.SetActive(false);
        ClearAllUAPs();
        SetGameOverState(FailScreen);
    }

    public void WinGame()
    {
        Game3DOFScene.SetActive(false);
        ClearAllUAPs(); // Specific requirement for Win
        SetGameOverState(WonScreen);
    }

    private void SetGameOverState(GameObject targetScreen)
    {
        gamePlaying = false;
        gamePaused = false;

        ToggleUI(targetScreen);
        laserPointer.SetActive(true);

        asteroidSpawner.StopSpawning();
        Time.timeScale = 0;
    }

    public void GoHome()
    {
        Time.timeScale = 1;

        ToggleUI(HomeScreen);
        Game3DOFScene.SetActive(false);

        gamePaused = false;
        gamePlaying = false;
        laserPointer.SetActive(false);

        asteroidSpawner.StopSpawning();
        ClearAllUAPs();
    }

    /// <summary>
    /// Optimized UI switcher: Disables all screens and enables only the target.
    /// </summary>
    private void ToggleUI(GameObject activeScreen)
    {
        foreach (GameObject screen in allScreens)
        {
            if (screen != null)
                screen.SetActive(screen == activeScreen);
        }
    }

    public void ClearAllUAPs()
    {
        GameObject[] uaps = GameObject.FindGameObjectsWithTag("UAP");
        for (int i = 0; i < uaps.Length; i++)
        {
            Destroy(uaps[i]);
        }
    }
}