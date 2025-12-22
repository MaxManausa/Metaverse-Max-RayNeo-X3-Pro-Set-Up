using UnityEngine;
using TMPro;
using System.Collections.Generic;

/*
 * MISSION CRITICAL: Handles mission telemetry and career grading.
 * Note: Career stats are purged upon calling StartOver() for a clean slate.
 */

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Managers")]
    [SerializeField] private SEDSceneManager sceneManager;

    [Header("Main Game UI")]
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

    private void Start()
    {
        StartOver();
    }

    public void AddDestroyedPoint()
    {
        if (isEndOfLevel || (sceneManager != null && !sceneManager.gamePlaying)) return;

        targetsDestroyed++;
        lifetimeTargetsDestroyed++;
        UpdateVisuals();

        if (targetsDestroyed >= targetsToWin)
        {
            EndLevel(true);
        }
    }

    // Choice A: damageValue is ignored, using internal random logic instead
    public void AddEarthHit(float damageValue = 0)
    {
        if (isEndOfLevel || (sceneManager != null && !sceneManager.gamePlaying)) return;

        float randomDamage = Random.Range(minDamagePerHit, maxDamagePerHit);
        currentEarthDamage += randomDamage;
        UpdateVisuals();

        if (currentEarthDamage >= maxEarthDamage)
        {
            currentEarthDamage = maxEarthDamage;
            EndLevel(false);
        }
    }

    private void EndLevel(bool won)
    {
        isEndOfLevel = true;
        levelScoreHistory.Add(currentEarthDamage);
        UpdateEndScreens();

        if (won) sceneManager.WinGame();
        else sceneManager.FailGame();
    }

    private void UpdateVisuals()
    {
        if (destroyedCountText != null)
            destroyedCountText.text = $"HITS: {targetsDestroyed}/{targetsToWin}";

        if (earthHitsCountText != null)
            earthHitsCountText.text = $"DMG: {Mathf.CeilToInt(currentEarthDamage)}%";
    }

    private void UpdateEndScreens()
    {
        int sessionPossible = lifetimePossibleTargets + targetsToWin;

        float averageScore = 0;
        if (levelScoreHistory.Count > 0)
        {
            float sum = 0;
            foreach (float s in levelScoreHistory) sum += s;
            averageScore = sum / levelScoreHistory.Count;
        }

        string lGrade = GetGradeLetter(currentEarthDamage);
        string tGrade = GetGradeLetter(averageScore);
        string lStats = $"{targetsDestroyed}/{targetsToWin}";
        string tStats = $"{lifetimeTargetsDestroyed}/{sessionPossible}";

        SetUI(winnerLevel, $"LEVEL {level}");
        SetUI(winnerStats, $"HITS: {lStats} | DMG: {Mathf.CeilToInt(currentEarthDamage)}%");
        SetUI(winnerGrade, $"LEVEL GRADE: {lGrade}");
        SetUI(winnerTotalScoreText, tStats);
        SetUI(winnerTotalGradeText, tGrade);

        SetUI(loserLevel, $"LEVEL {level}");
        SetUI(loserStats, $"HITS: {lStats} | DMG: {Mathf.CeilToInt(currentEarthDamage)}%");
        SetUI(loserGrade, $"LEVEL GRADE: {lGrade}");
        SetUI(loserTotalScoreText, tStats);
        SetUI(loserTotalGradeText, tGrade);

        lifetimePossibleTargets = sessionPossible;
    }

    private void SetUI(TextMeshProUGUI text, string content)
    {
        if (text != null) text.text = content;
    }

    private string GetGradeLetter(float score)
    {
        if (score <= 0) return "S";
        if (score <= 2) return "A+";
        if (score <= 7) return "A";
        if (score <= 15) return "B+";
        if (score <= 25) return "B";
        if (score <= 35) return "C+";
        if (score <= 45) return "C";
        if (score <= 60) return "D";
        return "F";
    }

    public void StartOver()
    {
        level = 1;
        targetsToWin = 25;
        isEndOfLevel = false;
        ResetLifetimeStats();

        SessionTimer timer = FindObjectOfType<SessionTimer>(true);
        if (timer != null) timer.ResetTimer();

        ResetVisuals();
    }

    public void LevelUp()
    {
        if (level >= MAX_LEVEL)
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            return;
        }

        level++;
        targetsToWin += 25;
        isEndOfLevel = false;
        ResetVisuals();
    }

    public void ResetLifetimeStats()
    {
        lifetimeTargetsDestroyed = 0;
        lifetimePossibleTargets = 0;
        levelScoreHistory.Clear();
    }

    public void ResetVisuals()
    {
        targetsDestroyed = 0;
        currentEarthDamage = 0f;
        UpdateVisuals();
    }
}