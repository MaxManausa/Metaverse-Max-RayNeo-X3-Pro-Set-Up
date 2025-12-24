using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    private TextMeshProUGUI fpsText;

    [Header("UI Settings")]
    [SerializeField] private GameObject connectionText; // Drag your red warning text here in the Inspector

    public float updateInterval = 0.5f;
    private float accum = 0;
    private int frames = 0;
    private float timeleft;

    void Start()
    {
        fpsText = GetComponent<TextMeshProUGUI>();
        timeleft = updateInterval;

        // Ensure connection text starts in the correct state
        if (connectionText != null)
            connectionText.SetActive(false);
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        if (timeleft <= 0.0)
        {
            float fps = accum / frames;

            // Apply color coding
            string color = (fps < 30) ? "<color=red>" : (fps < 50 ? "<color=yellow>" : "<color=green>");
            fpsText.text = color + Mathf.RoundToInt(fps) + " FPS</color>";

            // Logic for Low Mode and Connection Text
            HandlePerformanceMode(fps);

            // Reset counters
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
    }

    private void HandlePerformanceMode(float fps)
    {
        if (fps < 30)
        {
            LowMode();
        }
        else
        {
            NormalMode();   
        }
    }

    public void LowMode()
    {
        // Switch to Low Mode
        if (connectionText != null) connectionText.SetActive(true);

        // Optional: Insert code here to reduce graphics quality, e.g.:
        // QualitySettings.SetQualityLevel(0); 
        Debug.Log("Performance: LOW MODE ACTIVE");
    }

    public void NormalMode()
    {
        // Switch to Normal Mode
        if (connectionText != null) connectionText.SetActive(false);

        // Optional: Restore quality settings, e.g.:
        // QualitySettings.SetQualityLevel(2);
    }

}