using UnityEngine;
using TMPro;

public class DeviceBatteryDisplay : MonoBehaviour
{
    private TextMeshProUGUI batteryText;

    [Header("Tactical UI")]
    [SerializeField] private string label = "PWR:";

    void Start()
    {
        batteryText = GetComponent<TextMeshProUGUI>();
        // Update every 30 seconds to save resources
        InvokeRepeating(nameof(UpdateBatteryInfo), 0f, 30f);
    }

    void UpdateBatteryInfo()
    {
        float batteryLevel = SystemInfo.batteryLevel;

        // SystemInfo.batteryLevel returns -1.0 if it cannot be determined
        if (batteryLevel >= 0f)
        {
            int percentage = Mathf.RoundToInt(batteryLevel * 100);

            // Format: PWR/LVL: 085%
            // "D3" ensures it always shows 3 digits (e.g., 005% instead of 5%)
            // This prevents the UI from shifting/jittering.
            batteryText.text = $"{label} {percentage.ToString("D2")}%";
        }
        else
        {
            // Technical error code look
            batteryText.text = $"{label} ERR_NA";
        }
    }
}