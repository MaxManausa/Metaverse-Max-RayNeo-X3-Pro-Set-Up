using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("References")]
    [SerializeField] private SEDSceneManager sceneManager;
    [SerializeField] private TextMeshProUGUI destroyedCountText;
    [SerializeField] private TextMeshProUGUI earthHitsCountText;

    [Header("Game Rules")]
    public int asteroidsToWin = 20;
    public int maxEarthDamage = 100;

    [Header("Damage Settings")]
    public float minDamagePerHit = 5f;
    public float maxDamagePerHit = 15f;

    [Header("Current Statistics")]
    public int asteroidsDestroyed = 0;
    public float currentEarthDamage = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateVisuals();
    }

    public void AddDestroyedPoint()
    {
        // Only count if the game is actually playing
        if (sceneManager != null && !sceneManager.gamePlaying) return;

        asteroidsDestroyed++;
        UpdateVisuals();

        // Check for Win Condition
        if (asteroidsDestroyed >= asteroidsToWin)
        {
            sceneManager.WinGame();
        }
    }

    public void AddEarthHit()
    {
        if (sceneManager != null && !sceneManager.gamePlaying) return;

        // Calculate random damage based on public min/max
        float randomDamage = Random.Range(minDamagePerHit, maxDamagePerHit);
        currentEarthDamage += randomDamage;

        UpdateVisuals();

        // Check for Fail Condition
        if (currentEarthDamage >= maxEarthDamage)
        {
            currentEarthDamage = maxEarthDamage; // Clamp to 100 for UI
            UpdateVisuals();
            sceneManager.FailGame();
        }
    }

    private void UpdateVisuals()
    {
        if (destroyedCountText != null)
        {
            destroyedCountText.text = $"Asteroids Destroyed: {asteroidsDestroyed}/{asteroidsToWin}";
        }

        if (earthHitsCountText != null)
        {
            // Display as whole number for cleaner UI
            earthHitsCountText.text = $"Earth Damage: {Mathf.FloorToInt(currentEarthDamage)}%";
        }
    }

    public void ResetVisuals()
    {
        asteroidsDestroyed = 0;
        currentEarthDamage = 0f;
        UpdateVisuals();
    }
}