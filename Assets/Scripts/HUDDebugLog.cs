using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDDebugLog : MonoBehaviour
{
    private TextMeshProUGUI debugText;
    // Store the last few log messages
    private Queue<string> logMessages = new Queue<string>();
    public int maxLines = 5;

    void OnEnable()
    {
        debugText = GetComponent<TextMeshProUGUI>();
        // Subscribe to Unity's logging system events
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks when disabled
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Add new message to the queue
        string formattedLog = $"\n[{type}] {logString}";
        logMessages.Enqueue(formattedLog);

        // Keep the queue size manageable (e.g., last 5 lines)
        if (logMessages.Count > maxLines)
        {
            logMessages.Dequeue();
        }

        // Rebuild the display text
        debugText.text = string.Join("", logMessages.ToArray());
    }

    // You can call this function from other scripts to log custom messages easily
    public static void LogCustom(string message)
    {
        Debug.Log("HUD Custom Log: " + message);
    }
}

