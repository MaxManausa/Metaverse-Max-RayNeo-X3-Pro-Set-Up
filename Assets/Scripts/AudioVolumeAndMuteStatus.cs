using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AudioVolumeAndMuteStatus : MonoBehaviour
{
    private TextMeshProUGUI statusText;
    // We store the brightness internally as Unity doesn't let you read the OS setting easily.
    private float currentBrightnessLevel = 1.0f;

    void Start()
    {
        statusText = GetComponent<TextMeshProUGUI>();
        // Apply initial brightness setting to the screen
        Screen.brightness = currentBrightnessLevel;
        // Check the status periodically
        InvokeRepeating("UpdateAudioStatus", 0f, 1f);
    }

    void UpdateAudioStatus()
    {
        // Get the current volume level (0.0 to 1.0)
        float volumeLevel = AudioListener.volume;
        int volumePercent = Mathf.RoundToInt(volumeLevel * 100);

        // Determine mute status and color
        string muteStatus;
        string textColor;
        if (AudioListener.volume == 0 || AudioListener.pause)
        {
            muteStatus = "Muted 🔇";
            textColor = "<color=red>";
        }
        else
        {
            muteStatus = "Active 🔊";
            textColor = "<color=green>";
        }

        // Get the brightness level
        int brightnessPercent = Mathf.RoundToInt(currentBrightnessLevel * 100);

        // Format the output: Volume on top line, Mute status on second line, Brightness on third line
        // We use rich text tags to apply color to the whole block
        statusText.text =
            $"{textColor}Volume: {volumePercent}%\n" +
            $"Status: {muteStatus}\n" +
            $"Brightness: {brightnessPercent}%</color>";

        // Ensure your TextMeshPro UI element's Rect Transform is tall enough to show all lines.
    }

    // Optional: Public method to allow other scripts/buttons to change the brightness
    public void SetBrightness(float level)
    {
        // Clamp the value to a valid brightness range (0.1 is min visible)
        currentBrightnessLevel = Mathf.Clamp(level, 0.1f, 1.0f);
        Screen.brightness = currentBrightnessLevel;
    }
}