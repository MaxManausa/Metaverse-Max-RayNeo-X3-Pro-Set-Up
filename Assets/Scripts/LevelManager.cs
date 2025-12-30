using UnityEngine;
using TMPro;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public enum GameMode { Earth, Moon, TravelToMoon, TravelToEarth }

    [System.Serializable]
    public struct LevelConfig
    {
        public int levelNumber;
        public GameMode mode;
        public int asteroidCount;
        public int alienCount;
        public int warriorAlienCount;
        public int bossAlienCount;
        public string customMissionText;
    }

    [Header("HUD Sync References")]
    [SerializeField] private TextMeshProUGUI levelTitleText;
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private GameObject hudMarker;

    [Header("Settings")]
    [SerializeField] private float missionDisplayDuration = 5f;

    [Header("Environment Objects")]
    [SerializeField] private GameObject earthObject;
    [SerializeField] private GameObject moonObject;
    [SerializeField] private GameObject travelToMoonObject;
    [SerializeField] private GameObject travelToEarthObject;

    [Header("Current Level State")]
    public LevelConfig currentLevel;
    public Transform currentTargetTransform;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // FIX: Removed Invoke("StartFirstLevel", 0.2f);
        // This prevents the game from auto-starting Level 1 at the Home Menu.
        currentLevel.levelNumber = 0;
    }

    public void StartLevel(int level)
    {
        switch (level)
        {
            case 1: SetLevel(1, GameMode.Earth, 15, 0, 0, 0, "GOAL: PROTECT THE EARTH\nMISSION: Destroy 15 Asteroids"); break;
            case 2: SetLevel(2, GameMode.Earth, 40, 1, 0, 0, "GOAL: PROTECT THE EARTH\nMISSION: Destroy 40 Asteroids"); break;
            case 3: SetLevel(3, GameMode.Earth, 55, 5, 3, 0, "GOAL: PROTECT THE EARTH\nMISSION: Destroy 60 Asteroids"); break;
            case 4: SetLevel(4, GameMode.TravelToEarth, 55, 15, 0, 0, "GOAL: RECOVER SATELLITE\nMISSION: Destroy 60 Asteroids"); break;
            case 5: SetLevel(5, GameMode.Earth, 60, 15, 10, 3, "GOAL: PROTECT EARTH\nMISSION: Destroy 60 Asteroids"); break;
            case 6: SetLevel(6, GameMode.TravelToMoon, 65, 20, 0, 0, "GOAL: TRAVEL TO MOON\nMISSION: Destroy 65 Asteroids"); break;
            case 7: SetLevel(7, GameMode.Moon, 50, 15, 5, 0, "GOAL: PROTECT THE MOON\nMISSION: Destroy 60 Asteroids"); break;
            case 8: SetLevel(8, GameMode.Moon, 55, 20, 15, 5, "GOAL: DEFEND THE MOON\nMISSION: Destroy 60 Asteroids\nMISSION: Hold back Alien Armada"); break;
            case 9: SetLevel(9, GameMode.TravelToEarth, 65, 25, 0, 0, "GOAL: TRAVEL TO EARTH\nMISSION: Destroy 60 Asteroids"); break;
            case 10: SetLevel(10, GameMode.Earth, 80, 30, 25, 8, "GOAL: DEFEND THE EARTH\nMISSION: Destroy 80 Asteroids\nMISSION: Hold back Alien Armada"); break;
            default: Debug.LogWarning("Level not defined!"); return;
        }

        UpdateEnvironment(currentLevel.mode);
        UpdateLevelUI(level);

        StopAllCoroutines();
        StartCoroutine(MissionDisplayRoutine());

        if (ScoreManager.Instance != null) ScoreManager.Instance.LoadLevelData(level);
        TriggerSpawn();
    }

    private void SetLevel(int lvl, GameMode mode, int ast, int al, int war, int boss, string missionMsg)
    {
        currentLevel.levelNumber = lvl;
        currentLevel.mode = mode;
        currentLevel.asteroidCount = ast;
        currentLevel.alienCount = al;
        currentLevel.warriorAlienCount = war;
        currentLevel.bossAlienCount = boss;
        currentLevel.customMissionText = missionMsg;
    }

    private void UpdateLevelUI(int n)
    {
        if (levelTitleText != null) levelTitleText.text = "Level " + n;
        if (missionText != null) missionText.text = currentLevel.customMissionText;
    }

    private IEnumerator MissionDisplayRoutine()
    {
        ToggleMissionUI(true);
        yield return new WaitForSeconds(missionDisplayDuration);
        ToggleMissionUI(false);
    }

    public void ToggleMissionUI(bool showMissionText)
    {
        if (levelTitleText != null) levelTitleText.gameObject.SetActive(showMissionText);
        if (missionText != null) missionText.gameObject.SetActive(showMissionText);
        if (hudMarker != null) hudMarker.SetActive(!showMissionText);
    }

    private void UpdateEnvironment(GameMode mode)
    {
        if (earthObject) earthObject.SetActive(false);
        if (moonObject) moonObject.SetActive(false);
        if (travelToMoonObject) travelToMoonObject.SetActive(false);
        if (travelToEarthObject) travelToEarthObject.SetActive(false);

        switch (mode)
        {
            case GameMode.Earth:
                if (earthObject) { earthObject.SetActive(true); currentTargetTransform = earthObject.transform; }
                break;
            case GameMode.Moon:
                if (moonObject) { moonObject.SetActive(true); currentTargetTransform = moonObject.transform; }
                break;
            case GameMode.TravelToMoon:
                if (travelToMoonObject) { travelToMoonObject.SetActive(true); currentTargetTransform = travelToMoonObject.transform; }
                break;
            case GameMode.TravelToEarth:
                if (travelToEarthObject) { travelToEarthObject.SetActive(true); currentTargetTransform = travelToEarthObject.transform; }
                break;
        }
    }

    public void TriggerSpawn()
    {
        if (AsteroidSpawner.Instance != null) AsteroidSpawner.Instance.StartSpawning();
    }

    public void GoToNextLevel()
    {
        int nextLvl = currentLevel.levelNumber + 1;
        if (nextLvl <= 10) StartLevel(nextLvl);
    }
}