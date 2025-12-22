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
        InitializeLevel();
        scoreManager.ResetVisuals();
        if (satelliteController != null) satelliteController.SetZPosition(1000f);
    }

    public void NextLevel()
    {
        InitializeLevel();
        scoreManager.LevelUp();
    }

    private void InitializeLevel()
    {
        UpdateStates(playing: true, paused: false, ended: false);
        wonLevel = false;

        ToggleUI(GameUIScreen);
        Game3DOFScene.SetActive(true);
        resetHeadTrack.OnReset();
        laserPointer.SetActive(true);

        ClearAllUAPs();
        asteroidSpawner.StartSpawning();

        if (satelliteController != null)
        {
            satelliteController.SetZPosition(1000f);
            satelliteController.RestartOrbit();
        }

        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        if (!gamePlaying || gamePaused) return;

        UpdateStates(playing: false, paused: true, ended: false);

        // MANUALLY OVERRIDE TOGGLE: Keep Game UI on, turn Pause Screen on
        ToggleUI(PauseScreen);
        GameUIScreen.SetActive(true); // <--- Keep telemetry visible

        laserPointer.SetActive(true); // Usually you need the laser to click "Resume"
        asteroidSpawner.StopSpawning();
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        if (!gamePaused) return;

        UpdateStates(playing: true, paused: false, ended: false);

        ToggleUI(GameUIScreen); // This will hide the Pause screen

        laserPointer.SetActive(true);
        asteroidSpawner.StartSpawning();
        Time.timeScale = 1;
    }

    public void FailGame() => HandleGameOver(FailScreen, false);
    public void WinGame() => HandleGameOver(WonScreen, true);

    private void HandleGameOver(GameObject targetScreen, bool isWin)
    {
        UpdateStates(playing: false, paused: false, ended: true);
        wonLevel = isWin;

        Game3DOFScene.SetActive(false);
        ClearAllUAPs();
        ToggleUI(targetScreen);

        laserPointer.SetActive(true);
        asteroidSpawner.StopSpawning();
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
        asteroidSpawner.StopSpawning();
        ClearAllUAPs();

        if (scoreManager != null) scoreManager.StartOver();
        if (satelliteController != null) satelliteController.SetZPosition(1000f);
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
        GameObject[] uaps = GameObject.FindGameObjectsWithTag("UAP");
        for (int i = 0; i < uaps.Length; i++)
        {
            if (uaps[i] != null) Destroy(uaps[i]);
        }
    }
}