using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// --- RENAME THE CLASS HERE ---
public class DeviceBatteryDisplay : MonoBehaviour
{
    private TextMeshProUGUI batteryText;

    void Start()
    {
        batteryText = GetComponent<TextMeshProUGUI>();
        InvokeRepeating("UpdateBatteryInfo", 0f, 30f);
    }

    void UpdateBatteryInfo()
    {
        float batteryLevel = SystemInfo.batteryLevel;

        if (batteryLevel > 0f)
        {
            int percentage = Mathf.RoundToInt(batteryLevel * 100);

            // --- This line now works correctly ---
            string status = SystemInfo.batteryStatus == BatteryStatus.Charging ? "Charging" : "Not Charging";

            batteryText.text = $"{percentage}% ({status})";
        }
        else
        {
            batteryText.text = "Battery N/A";
        }
    }
}
