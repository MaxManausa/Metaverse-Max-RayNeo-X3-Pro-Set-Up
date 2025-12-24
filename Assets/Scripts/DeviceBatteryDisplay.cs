using UnityEngine;
using TMPro;
using System.Collections;

public class DeviceBatteryDisplay : MonoBehaviour
{
    private TextMeshProUGUI batteryText;

    [Header("Tactical UI")]
    [SerializeField] private string label = "PWR:";
    [SerializeField] private float updateInterval = 30f;

    void Awake()
    {
        batteryText = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        // Start the loop every time the object is activated
        StartCoroutine(BatteryUpdateRoutine());
    }

    void OnDisable()
    {
        // Stop the loop when the object is deactivated to save battery
        StopAllCoroutines();
    }

    private IEnumerator BatteryUpdateRoutine()
    {
        // Infinite loop that runs as long as the object is active
        while (true)
        {
            UpdateBatteryInfo();

            // WaitForSecondsRealtime is NOT affected by Time.timeScale = 0
            yield return new WaitForSecondsRealtime(updateInterval);
        }
    }

    void UpdateBatteryInfo()
    {
        if (batteryText == null) return;

        float batteryLevel = SystemInfo.batteryLevel;

        // SystemInfo.batteryLevel returns -1.0 if it cannot be determined (like in Editor)
        if (batteryLevel >= 0f)
        {
            int percentage = Mathf.RoundToInt(batteryLevel * 100);

            // Format ensures it stays fixed-width to prevent UI jitter
            batteryText.text = $"{label} {percentage.ToString("D2")}%";
        }
        else
        {
            // Fallback for Editor testing or unsupported devices
            batteryText.text = $"{label} --%";
        }
    }
}