using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SystemInfoDisplay : MonoBehaviour
{
    private TextMeshProUGUI infoText;

    void Start()
    {
        infoText = GetComponent<TextMeshProUGUI>();
        DisplayAllInfo();
    }

    void DisplayAllInfo()
    {
        // 6. Operating System Version
        string osInfo = $"OS: {SystemInfo.operatingSystem}\n";

        // 7. Processor Info
        string cpuInfo = $"CPU: {SystemInfo.processorType} ({SystemInfo.processorCount} cores)\n";

        // 8. VRAM/GPU Info
        string gpuInfo = $"GPU: {SystemInfo.graphicsDeviceName}\n";

        // 9. Screen Resolution
        string resolutionInfo = $"Res: {Screen.currentResolution.width}x{Screen.currentResolution.height}\n";

        // 10. Application Version Number
        string appVersionInfo = $"App Version: {Application.version}";

        // Combine all strings using Newline characters (\n)
        infoText.text = osInfo + cpuInfo + gpuInfo + resolutionInfo + appVersionInfo;

        // Ensure your TextMeshPro UI element's Rect Transform is large enough
        // and Wrap Mode is enabled in the Inspector to display all lines.
    }
}
