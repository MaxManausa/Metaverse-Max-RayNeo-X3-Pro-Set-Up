using UnityEngine;
using TMPro;
using System;

public class SessionTimer : MonoBehaviour
{
    private TextMeshProUGUI timerText;

    // Static ensures this value stays the same across all instances 
    // and survives the script being disabled/enabled.
    private static DateTime startTime;
    private static bool startTimeSet = false;

    void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();

        // Set the start time only once per game session
        if (!startTimeSet)
        {
            startTime = DateTime.Now;
            startTimeSet = true;
        }
    }

    // This allows you to manually restart the session clock if needed
    public void ResetTimer()
    {
        startTime = DateTime.Now;
    }

    void Update()
    {
        if (timerText == null) return;

        // Calculate time passed since the game started, 
        // regardless of Time.timeScale or Active/Inactive status.
        TimeSpan t = DateTime.Now - startTime;

        // Format: Hours:Minutes:Seconds
        timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
            (int)t.TotalHours,
            t.Minutes,
            t.Seconds);
    }
}