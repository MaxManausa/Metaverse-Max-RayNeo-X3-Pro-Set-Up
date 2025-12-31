using System.Collections;
using TMPro;
using UnityEngine;

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
        public string missionClean;
        public string goalClean;
        public string tipClean;
    }

    [Header("Popup HUD (These Go Inactive)")]
    [SerializeField] private TextMeshProUGUI levelTitleText;
    [SerializeField] private TextMeshProUGUI popupMissionText;
    [SerializeField] private TextMeshProUGUI popupGoalText;
    [SerializeField] private TextMeshProUGUI popupTipText;
    [SerializeField] private GameObject hudMarker;

    [Header("Parallel Display (Always Active)")]
    [SerializeField] private TextMeshProUGUI parallelMissionText;
    [SerializeField] private TextMeshProUGUI parallelGoalText;
    [SerializeField] private TextMeshProUGUI parallelTipText;

    [Header("Settings")]
    [SerializeField] private float missionDisplayDuration = 5f;
    [SerializeField] private float tipExtraDuration = 2f;

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
        currentLevel.levelNumber = 0;
    }

    public void StartLevel(int level)
    {
        // Check for Game Completion
        if (level > 99)
        {
            Debug.Log("Level 99 Cleared. Closing Game.");
            Application.Quit();
            return;
        }

        // Logic Index maps levels 11, 21, 31... to Case 1
        // (level - 1) % 10 + 1 ensures the result is always between 1 and 10
        int logicIndex = (level - 1) % 10 + 1;

        switch (logicIndex)
        {
            case 1: SetLevel(level, GameMode.Earth, 15, 0, 0, 0, "PROTECT THE EARTH", "Destroy 15 Asteroids", "Move head to hit targets"); break;
            case 2: SetLevel(level, GameMode.Earth, 40, 1, 0, 0, "PROTECT THE EARTH", "Destroy 40 Asteroids", "Destroy all potential threats"); break;
            case 3: SetLevel(level, GameMode.Earth, 55, 5, 3, 0, "PROTECT THE EARTH", "Destroy 60 Asteroids", "Combat UAPs require more focus"); break;
            case 4: SetLevel(level, GameMode.TravelToEarth, 55, 15, 0, 0, "RECOVER UAP", "Destroy 60 Asteroids", "Protect the asset"); break;
            case 5: SetLevel(level, GameMode.Earth, 60, 15, 10, 3, "PROTECT EARTH", "Destroy 60 Asteroids", "Beware long-range UAPs"); break;
            case 6: SetLevel(6, GameMode.TravelToMoon, 65, 20, 0, 0, "TRAVEL TO MOON", "Destroy 65 Asteroids", "Be ready."); break;
            case 7: SetLevel(level, GameMode.Moon, 50, 15, 5, 0, "PROTECT THE MOON", "Destroy 60 Asteroids", "Take out long-range UAPs ASAP"); break;
            case 8: SetLevel(level, GameMode.Moon, 55, 20, 15, 5, "DEFEND THE MOON", "Destroy 60 Asteroids\nHold back Alien Armada", "Don't be overwhelmed."); break;
            case 9: SetLevel(level, GameMode.TravelToMoon, 65, 25, 0, 0, "TRAVEL TO EARTH", "Destroy 60 Asteroids", "Almost home - Stay alert!"); break;
            case 10: SetLevel(level, GameMode.Earth, 80, 30, 25, 8, "DEFEND THE EARTH", "Destroy 80 Asteroids\nHold back Alien Armada", "Defend Earth at all costs!"); break;
            default: Debug.LogWarning("Level not defined!"); return;
        }

        UpdateEnvironment(currentLevel.mode);
        UpdateLevelUI(level);

        StopAllCoroutines();
        StartCoroutine(MissionDisplayRoutine());

        var sm = FindFirstObjectByType<ScoreManager>();
        if (sm != null) sm.LoadLevelData(level);

        TriggerSpawn();
    }

    private void SetLevel(int lvl, GameMode mode, int ast, int al, int war, int boss, string mission, string goal, string tip)
    {
        currentLevel.levelNumber = lvl;
        currentLevel.mode = mode;
        currentLevel.asteroidCount = ast;
        currentLevel.alienCount = al;
        currentLevel.warriorAlienCount = war;
        currentLevel.bossAlienCount = boss;

        currentLevel.missionClean = mission;
        currentLevel.goalClean = goal;
        currentLevel.tipClean = tip;
    }

    private void UpdateLevelUI(int n)
    {
        if (levelTitleText != null) levelTitleText.text = "Level " + n;
        if (popupMissionText != null) popupMissionText.text = "MISSION: " + currentLevel.missionClean;
        if (popupGoalText != null) popupGoalText.text = "GOAL: " + currentLevel.goalClean;
        if (popupTipText != null) popupTipText.text = "TIP: " + currentLevel.tipClean;

        if (parallelMissionText != null) parallelMissionText.text = currentLevel.missionClean;
        if (parallelGoalText != null) parallelGoalText.text = currentLevel.goalClean;
        if (parallelTipText != null) parallelTipText.text = currentLevel.tipClean;
    }

    private IEnumerator MissionDisplayRoutine()
    {
        TogglePopupUI(true);
        if (popupTipText != null) popupTipText.gameObject.SetActive(true);

        yield return new WaitForSeconds(missionDisplayDuration);

        TogglePopupUI(false);
        yield return new WaitForSeconds(tipExtraDuration);

        if (popupTipText != null) popupTipText.gameObject.SetActive(false);
    }

    public void TogglePopupUI(bool show)
    {
        if (levelTitleText != null) levelTitleText.gameObject.SetActive(show);
        if (popupMissionText != null) popupMissionText.gameObject.SetActive(show);
        if (popupGoalText != null) popupGoalText.gameObject.SetActive(show);
        if (hudMarker != null) hudMarker.SetActive(!show);
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
        var spawner = FindFirstObjectByType<AsteroidSpawner>();
        if (spawner != null) spawner.StartSpawning();
    }

    public void GoToNextLevel()
    {
        int nextLvl = currentLevel.levelNumber + 1;

        // Removed the "if <= 10" restriction to allow looping to level 99
        StartLevel(nextLvl);
    }
}