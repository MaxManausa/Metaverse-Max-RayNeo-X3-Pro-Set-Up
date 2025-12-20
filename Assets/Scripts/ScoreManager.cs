using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Changed from UnityEngine.UI to TMPro

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Statistics")]
    public int asteroidsDestroyed = 0;
    public int earthHits = 0;

    [Header("UI Text References")]
    // Changed type from Text to TextMeshProUGUI
    [SerializeField] private TextMeshProUGUI destroyedCountText;
    [SerializeField] private TextMeshProUGUI earthHitsCountText;

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
        asteroidsDestroyed++;
        UpdateVisuals();
        Debug.Log("Asteroids Destroyed: " + asteroidsDestroyed);
    }

    public void AddEarthHit()
    {
        earthHits++;
        UpdateVisuals();
        Debug.Log("Earth Hits: " + earthHits);
    }

    private void UpdateVisuals()
    {
        if (destroyedCountText != null)
        {
            destroyedCountText.text = "Asteroids Destroyed: " + asteroidsDestroyed;
        }

        if (earthHitsCountText != null)
        {
            // Keeping your percentage logic
            earthHitsCountText.text = "Earth Damage: " + (earthHits * 10) + "%";
        }
    }

    public void ResetVisuals()
    {
        asteroidsDestroyed = 0;
        destroyedCountText.text = "Asteroids Destroyed: " + asteroidsDestroyed;
        earthHits = 0;  
        earthHitsCountText.text = "Earth Damage: " + earthHits + "%";
    }
}