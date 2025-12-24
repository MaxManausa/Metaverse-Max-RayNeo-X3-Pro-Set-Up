using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Managers")]
    [SerializeField] private SEDSceneManager sceneManager;
    [SerializeField] private SEDVideoPlayer sedVideoPlayer;

    [Header("HUD Sync References")]
    [SerializeField] private TextMeshProUGUI lvlText;         // The small "LVL: 1" text
    [SerializeField] private TextMeshProUGUI levelTitleText;   // The large "Level 1" title
    [SerializeField] private TextMeshProUGUI tipsText;        // Left side tips
    [SerializeField] private TextMeshProUGUI missionText;     // Right side mission objectives

    [Header("UI References (Telemetry)")]
    [SerializeField] private TextMeshProUGUI destroyedCountText;
    [SerializeField] private TextMeshProUGUI earthHitsCountText;

    [Header("End Screen - Level Stats")]
    [SerializeField] private TextMeshProUGUI winnerLevel;
    [SerializeField] private TextMeshProUGUI loserLevel;
    [SerializeField] private TextMeshProUGUI winnerStats;
    [SerializeField] private TextMeshProUGUI loserStats;
    [SerializeField] private TextMeshProUGUI winnerGrade;
    [SerializeField] private TextMeshProUGUI loserGrade;

    [Header("End Screen - Career Stats")]
    [SerializeField] private TextMeshProUGUI winnerTotalScoreText;
    [SerializeField] private TextMeshProUGUI loserTotalScoreText;
    [SerializeField] private TextMeshProUGUI winnerTotalGradeText;
    [SerializeField] private TextMeshProUGUI loserTotalGradeText;

    [Header("Game Rules")]
    public int level = 1;
    public const int MAX_LEVEL = 10;
    public int targetsToWin = 25;
    public int maxEarthDamage = 100;

    [Header("Damage Settings")]
    public float minDamagePerHit = 10f;
    public float maxDamagePerHit = 15f;

    [Header("Current Level Stats")]
    public int targetsDestroyed = 0;
    public float currentEarthDamage = 0f;
    private bool isEndOfLevel = false;
    private bool isPaused = true;

    [Header("Lifetime Career Stats")]
    public int lifetimeTargetsDestroyed = 0;
    public int lifetimePossibleTargets = 0;
    private List<float> levelScoreHistory = new List<float>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (levelScoreHistory == null) levelScoreHistory = new List<float>();
    }

    private void Update()
    {
        if (isPaused || isEndOfLevel) return;
        UpdateTelemetryUI();
    }

    // --- Level Sync Methods ---

    private void SetLevelData(int n, string tips, string mission)
    {
        level = n;

        // Debug check: If you see this in the Console but not on screen, 
        // you have the wrong object assigned in the Inspector!
        Debug.Log($"<color=cyan>ScoreManager: Setting UI for Level {n}</color>");

        if (lvlText != null) lvlText.text = "LVL: " + n;
        if (levelTitleText != null) levelTitleText.text = "Level " + n;
        if (tipsText != null) tipsText.text = tips;
        if (missionText != null) missionText.text = mission;

        if (sedVideoPlayer != null)
        {
            sedVideoPlayer.Invoke("Level" + n + "Video", 0);
        }
    }

    public void SetupLevel1() => SetLevelData(1, "- Look at asteroids to fire laser", "- DESTROY 25 ASTEROIDS\n- PROTECT EARTH");
    public void SetupLevel2() => SetLevelData(2, "- Asteroids are getting faster!", "- DESTROY 40 ASTEROIDS\n- PROTECT EARTH");
    public void SetupLevel3() => SetLevelData(3, "- Multiple waves detected", "- DESTROY 55 ASTEROIDS\n- PROTECT EARTH");
    public void SetupLevel4() => SetLevelData(4, "- Shields holding... for now", "- DESTROY 70 ASTEROIDS\n- PROTECT EARTH");
    public void SetupLevel5() => SetLevelData(5, "- Halfway there, Pilot!", "- DESTROY 85 ASTEROIDS\n- PROTECT EARTH");
    public void SetupLevel6() => SetLevelData(6, "- Scanners showing heavy debris", "- DESTROY 100 ASTEROIDS\n- PROTECT EARTH");
    public void SetupLevel7() => SetLevelData(7, "- Stay focused on the horizon", "- DESTROY 115 ASTEROIDS\n- PROTECT EARTH");
    public void SetupLevel8() => SetLevelData(8, "- Atmosphere is taking heat", "- DESTROY 130 ASTEROIDS\n- PROTECT EARTH");
    public void SetupLevel9() => SetLevelData(9, "- Final defensive perimeter!", "- DESTROY 145 ASTEROIDS\n- PROTECT EARTH");
    public void SetupLevel10() => SetLevelData(10, "- THIS IS THE END", "- DESTROY 160 ASTEROIDS\n- PROTECT EARTH");

    public void StartOver()
    {
        targetsToWin = 25;
        lifetimeTargetsDestroyed = 0;
        lifetimePossibleTargets = targetsToWin;
        levelScoreHistory.Clear();
        ResetLevelStats();

        SetupLevel1();
        UpdateTelemetryUI();
    }

    public void LevelUp()
    {
        if (level >= MAX_LEVEL) return;

        level++;
        targetsToWin += 15;
        lifetimePossibleTargets += targetsToWin;
        ResetLevelStats();

        // Direct switch is much safer than Invoke string
        switch (level)
        {
            case 2: SetupLevel2(); break;
            case 3: SetupLevel3(); break;
            case 4: SetupLevel4(); break;
            case 5: SetupLevel5(); break;
            case 6: SetupLevel6(); break;
            case 7: SetupLevel7(); break;
            case 8: SetupLevel8(); break;
            case 9: SetupLevel9(); break;
            case 10: SetupLevel10(); break;
        }

        UpdateTelemetryUI();
    }

    public void UpdateTelemetryUI()
    {
        if (destroyedCountText != null) destroyedCountText.text = $"HITS: {targetsDestroyed}/{targetsToWin}";
        if (earthHitsCountText != null) earthHitsCountText.text = $"DMG: {Mathf.CeilToInt(currentEarthDamage)}%";
    }

    public void UpdateEndScreens()
    {
        int sessionPossible = lifetimePossibleTargets;
        float averageDamage = 0;
        if (levelScoreHistory.Count > 0)
        {
            float sum = 0;
            foreach (float s in levelScoreHistory) sum += s;
            averageDamage = sum / levelScoreHistory.Count;
        }

        string lGrade = GetGradeLetter(currentEarthDamage);
        string tGrade = GetGradeLetter(averageDamage);
        string lStats = $"{targetsDestroyed}/{targetsToWin}";
        string tStats = $"{lifetimeTargetsDestroyed}/{sessionPossible}";

        SetUI(winnerLevel, $"LEVEL {level}");
        SetUI(winnerStats, $"HITS: {lStats} | DMG: {Mathf.CeilToInt(currentEarthDamage)}%");
        SetUI(winnerGrade, $"LEVEL GRADE: {lGrade}");

        SetUI(loserLevel, $"LEVEL {level}");
        SetUI(loserStats, $"HITS: {lStats} | DMG: {Mathf.CeilToInt(currentEarthDamage)}%");
        SetUI(loserGrade, $"LEVEL GRADE: {lGrade}");

        SetUI(winnerTotalScoreText, tStats);
        SetUI(winnerTotalGradeText, tGrade);
        SetUI(loserTotalScoreText, tStats);
        SetUI(loserTotalGradeText, tGrade);
    }

    private void SetUI(TextMeshProUGUI text, string content)
    {
        if (text != null) text.text = content;
    }

    private string GetGradeLetter(float score)
    {
        if (score <= 0) return "S";
        if (score <= 5) return "A+";
        if (score <= 12) return "A";
        if (score <= 25) return "B";
        if (score <= 45) return "C";
        return "F";
    }

    private void ResetLevelStats()
    {
        targetsDestroyed = 0;
        currentEarthDamage = 0f;
        isEndOfLevel = false;
    }

    public void SetPause(bool pauseState) => isPaused = pauseState;
    public void ResetVisuals() => UpdateTelemetryUI();

    public void AddDestroyedPoint()
    {
        if (isEndOfLevel || isPaused) return;
        targetsDestroyed++;
        lifetimeTargetsDestroyed++;
        UpdateTelemetryUI();
        if (targetsDestroyed >= targetsToWin) EndLevel(true);
    }

    public void AddEarthHit(float damageValue = 0)
    {
        if (isEndOfLevel || isPaused) return;
        float finalDamage = (damageValue > 0) ? damageValue : Random.Range(minDamagePerHit, maxDamagePerHit);
        currentEarthDamage += finalDamage;
        UpdateTelemetryUI();
        if (currentEarthDamage >= maxEarthDamage) { currentEarthDamage = maxEarthDamage; EndLevel(false); }
    }

    private void EndLevel(bool won)
    {
        isEndOfLevel = true;
        levelScoreHistory.Add(currentEarthDamage);
        if (won) sceneManager.WinGame();
        else sceneManager.FailGame();
    }
}