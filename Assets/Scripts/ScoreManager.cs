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
    [SerializeField] private TextMeshProUGUI lvlText;

    [Header("UI References (Telemetry)")]
    [SerializeField] private TextMeshProUGUI destroyedCountText;
    [SerializeField] private TextMeshProUGUI earthHitsCountText;

    [Header("End Screen Stats (Winner)")]
    [SerializeField] private TextMeshProUGUI winnerLevel;
    [SerializeField] private TextMeshProUGUI winnerStats;
    [SerializeField] private TextMeshProUGUI winnerGrade;
    [SerializeField] private TextMeshProUGUI winnerTotalStats;
    [SerializeField] private TextMeshProUGUI winnerTotalGrade;

    [Header("End Screen Stats (Loser)")]
    [SerializeField] private TextMeshProUGUI loserLevel;
    [SerializeField] private TextMeshProUGUI loserStats;
    [SerializeField] private TextMeshProUGUI loserGrade;
    [SerializeField] private TextMeshProUGUI loserTotalStats;
    [SerializeField] private TextMeshProUGUI loserTotalGrade;

    [Header("Career Stats")]
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI totalGradeText;

    [Header("Game Rules")]
    public int level = 0; // FIX: Changed from 1 to 0 to prevent menu spawning
    public int targetsToWin = 0;
    public int maxEarthDamage = 100;

    [Header("Current Level Stats")]
    public int targetsDestroyed = 0;
    public float currentEarthDamage = 0f;
    private bool isEndOfLevel = false;
    private bool isPaused = false;

    [Header("Lifetime Career Stats")]
    public int lifetimeTargetsDestroyed = 0;
    public int lifetimePossibleTargets = 0;
    private List<float> levelScoreHistory = new List<float>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (isPaused || isEndOfLevel) return;
        UpdateTelemetryUI();
    }

    public void LoadLevelData(int n)
    {
        level = n;
        ResetLevelStats();

        if (LevelMaterialController.Instance != null)
        {
            LevelMaterialController.Instance.SetLevel(n);
        }

        if (LevelManager.Instance != null)
        {
            var config = LevelManager.Instance.currentLevel;

            // SYNCHRONIZED CALCULATION
            targetsToWin = config.asteroidCount + config.alienCount +
                           config.warriorAlienCount + config.bossAlienCount;

            if (lvlText != null) lvlText.text = "LVL: " + n;
        }

        if (sedVideoPlayer != null) sedVideoPlayer.Invoke("Level" + n + "Video", 0);
        UpdateTelemetryUI();
    }

    public void StartOver()
    {
        level = 1;
        lifetimeTargetsDestroyed = 0;
        lifetimePossibleTargets = 0;
        levelScoreHistory.Clear();
        ResetLevelStats();

        if (LevelManager.Instance != null) LevelManager.Instance.StartLevel(1);
    }

    public void LevelUp()
    {
        if (LevelManager.Instance != null) LevelManager.Instance.GoToNextLevel();
    }

    public void AddDestroyedPoint()
    {
        if (isEndOfLevel || isPaused) return;
        targetsDestroyed++;
        lifetimeTargetsDestroyed++;
        UpdateTelemetryUI();
    }

    public void AddEarthHit(float damageValue = 0)
    {
        if (isEndOfLevel || isPaused) return;

        float finalDamage = (damageValue > 0) ? damageValue : Random.Range(10f, 15f);
        currentEarthDamage += finalDamage;

        UpdateTelemetryUI();

        if (currentEarthDamage >= maxEarthDamage)
        {
            currentEarthDamage = maxEarthDamage;
            EndLevel(false);
        }
    }

    public void EndLevel(bool won)
    {
        if (isEndOfLevel) return;
        isEndOfLevel = true;

        levelScoreHistory.Add(currentEarthDamage);
        lifetimePossibleTargets += targetsToWin;

        UpdateEndScreens();

        if (won) sceneManager.WinGame();
        else sceneManager.FailGame();
    }

    public void UpdateEndScreens()
    {
        string lGrade = GetGradeLetter(currentEarthDamage);
        string lStats = $"{targetsDestroyed}/{targetsToWin}";
        string tStats = $"{lifetimeTargetsDestroyed}/{lifetimePossibleTargets}";

        float avgDmg = 0;
        foreach (float f in levelScoreHistory) avgDmg += f;
        if (levelScoreHistory.Count > 0) avgDmg /= levelScoreHistory.Count;
        string tGrade = GetGradeLetter(avgDmg);

        if (winnerLevel != null) winnerLevel.text = $"LEVEL {level}";
        if (winnerStats != null) winnerStats.text = $"HITS: {lStats} | DMG: {Mathf.CeilToInt(currentEarthDamage)}%";
        if (winnerGrade != null) winnerGrade.text = $"GRADE: {lGrade}";
        if (winnerTotalStats != null) winnerTotalStats.text = $"TOTAL HITS: {tStats}";
        if (winnerTotalGrade != null) winnerTotalGrade.text = $"TOTAL GRADE: {tGrade}";

        if (loserLevel != null) loserLevel.text = $"LEVEL {level}";
        if (loserStats != null) loserStats.text = $"HITS: {lStats} | DMG: {Mathf.CeilToInt(currentEarthDamage)}%";
        if (loserGrade != null) loserGrade.text = $"GRADE: {lGrade}";
        if (loserTotalStats != null) loserTotalStats.text = $"TOTAL HITS: {tStats}";
        if (loserTotalGrade != null) loserTotalGrade.text = $"TOTAL GRADE: {tGrade}";

        if (totalScoreText != null) totalScoreText.text = tStats;
        if (totalGradeText != null) totalGradeText.text = tGrade;
    }

    public void UpdateTelemetryUI()
    {
        if (destroyedCountText != null) destroyedCountText.text = $"HITS: {targetsDestroyed}/{targetsToWin}";
        if (earthHitsCountText != null) earthHitsCountText.text = $"DMG: {Mathf.CeilToInt(currentEarthDamage)}%";
    }

    private void ResetLevelStats()
    {
        targetsDestroyed = 0;
        currentEarthDamage = 0f;
        isEndOfLevel = false;
    }

    public void SetPause(bool pauseState) => isPaused = pauseState;

    private string GetGradeLetter(float dmg)
    {
        if (dmg <= 5) return "S";
        if (dmg <= 15) return "A";
        if (dmg <= 30) return "B";
        if (dmg <= 50) return "C";
        return "F";
    }
}