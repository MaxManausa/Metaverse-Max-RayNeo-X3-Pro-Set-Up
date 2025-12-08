using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MemoryMonitor : MonoBehaviour
{
    private TextMeshProUGUI memoryText;

    void Start()
    {
        memoryText = GetComponent<TextMeshProUGUI>();
        InvokeRepeating("UpdateMemoryUsage", 0f, 5f); // Check memory every 5 seconds
    }

    void UpdateMemoryUsage()
    {
        // Get the total memory currently allocated to the application (in bytes)
        long totalMemory = GC.GetTotalMemory(false);

        // Convert to Megabytes for readability (1024 * 1024 bytes in a MB)
        float memoryMB = (float)totalMemory / (1024f * 1024f);

        memoryText.text = $"Mem: {memoryMB:F1} MB";
    }
}
