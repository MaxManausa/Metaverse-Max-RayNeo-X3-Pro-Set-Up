using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEDSceneManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private ResetHeadTrack resetHeadTrack;
    [SerializeField] private AsteroidSpawner asteroidSpawner;
    [SerializeField] private SatelliteOrbitController satelliteController;

    [Header("UI Screens")]
    [SerializeField] private GameObject HomeScreen;
    [SerializeField] private GameObject GameUIScreen;
    [SerializeField] private GameObject Game3DOFScene;
    [SerializeField] private GameObject PauseScreen;
    [SerializeField] private GameObject FailScreen;
    [SerializeField] private GameObject WonScreen;

    [Header("Tools")]
    [SerializeField] private GameObject laserPointer;

    private GameObject[] _allScreens;

    [HideInInspector] public bool gamePlaying { get; private set; }
    [HideInInspector] public bool gamePaused { get; private set; }
    [HideInInspector] public bool gameOver { get; private set; }
    [HideInInspector] public bool wonLevel { get; private set; }

    void Awake()
    {
        _allScreens = new GameObject[] { HomeScreen, GameUIScreen, PauseScreen, FailScreen, WonScreen };
    }

    void Start()
    {
        Time.timeScale = 1;
        GoHome();
    }

    public void StartGame()
    {
        // 1. Reset Career Stats and start Level 1 in LevelManager
        if (scoreManager != null) scoreManager.StartOver();

        // 2. Setup the visual and physics state
        InitializeLevel();

        if (satelliteController != null) satelliteController.SetZPosition(1000f);
    }

    public void NextLevel()
    {
        // 1. Tell ScoreManager to increment level, which calls LevelManager.GoToNextLevel()
        if (scoreManager != null) scoreManager.LevelUp();

        // 2. Refresh the level state
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        UpdateStates(playing: true, paused: false, ended: false);
        wonLevel = false;

        ToggleUI(GameUIScreen);
        Game3DOFScene.SetActive(true);

        if (resetHeadTrack != null) resetHeadTrack.OnReset();
        laserPointer.SetActive(true);

        // Crucial: Clear scene before spawning new wave
        ClearAllUAPs();

        if (asteroidSpawner != null) asteroidSpawner.StartSpawning();

        if (satelliteController != null)
        {
            satelliteController.SetZPosition(1000f);
            satelliteController.RestartOrbit();
        }

        Time.timeScale = 1;
        if (scoreManager != null) scoreManager.SetPause(false);
    }

    public void PauseGame()
    {
        if (!gamePlaying || gamePaused) return;
        UpdateStates(playing: false, paused: true, ended: false);
        ToggleUI(PauseScreen);
        GameUIScreen.SetActive(true);
        laserPointer.SetActive(true);

        if (asteroidSpawner != null) asteroidSpawner.StopSpawning();

        Time.timeScale = 0;
        if (scoreManager != null) scoreManager.SetPause(true);
    }

    public void ContinueGame()
    {
        if (!gamePaused) return;
        UpdateStates(playing: true, paused: false, ended: false);
        ToggleUI(GameUIScreen);
        laserPointer.SetActive(true);

        if (asteroidSpawner != null) asteroidSpawner.StartSpawning();

        Time.timeScale = 1;
        if (scoreManager != null) scoreManager.SetPause(false);
    }

    public void FailGame() => HandleGameOver(FailScreen, false);
    public void WinGame() => HandleGameOver(WonScreen, true);

    private void HandleGameOver(GameObject targetScreen, bool isWin)
    {
        UpdateStates(playing: false, paused: false, ended: true);
        wonLevel = isWin;

        if (scoreManager != null)
        {
            scoreManager.SetPause(true);
            scoreManager.UpdateEndScreens();
        }

        Game3DOFScene.SetActive(false);
        ClearAllUAPs();
        ToggleUI(targetScreen);

        laserPointer.SetActive(true);
        if (asteroidSpawner != null) asteroidSpawner.StopSpawning();

        Time.timeScale = 0;
    }

    public void GoHome()
    {
        Time.timeScale = 1;
        UpdateStates(playing: false, paused: false, ended: false);
        wonLevel = false;
        ToggleUI(HomeScreen);
        Game3DOFScene.SetActive(false);
        laserPointer.SetActive(false);

        // --- FIX ADDED HERE ---
        // Explicitly set the level back to 0 so the Spawner safety check triggers
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.currentLevel.levelNumber = 0;
        }
        // -----------------------

        if (asteroidSpawner != null) asteroidSpawner.StopSpawning();

        ClearAllUAPs();

        if (satelliteController != null) satelliteController.SetZPosition(1000f);
        if (scoreManager != null) scoreManager.SetPause(true);
    }

    private void UpdateStates(bool playing, bool paused, bool ended)
    {
        gamePlaying = playing;
        gamePaused = paused;
        gameOver = ended;
    }

    private void ToggleUI(GameObject activeScreen)
    {
        for (int i = 0; i < _allScreens.Length; i++)
        {
            if (_allScreens[i] != null)
                _allScreens[i].SetActive(_allScreens[i] == activeScreen);
        }
    }

    public void ClearAllUAPs()
    {
        // 1. Clear objects tagged UAP
        GameObject[] uaps = GameObject.FindGameObjectsWithTag("UAP");
        for (int i = 0; i < uaps.Length; i++)
        {
            if (uaps[i] != null) Destroy(uaps[i]);
        }

        // 2. Clear all objects using the AsteroidManager script
        AsteroidManager[] enemies = Object.FindObjectsByType<AsteroidManager>(FindObjectsSortMode.None);
        foreach (AsteroidManager enemy in enemies)
        {
            if (enemy != null) Destroy(enemy.gameObject);
        }

        // 3. Clear all Boss Blaster Bolts
        BlasterBolt[] strayBolts = Object.FindObjectsByType<BlasterBolt>(FindObjectsSortMode.None);
        foreach (BlasterBolt bolt in strayBolts)
        {
            if (bolt != null) Destroy(bolt.gameObject);
        }
    }
}