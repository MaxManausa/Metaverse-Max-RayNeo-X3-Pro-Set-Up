using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

public class LocaleDisplay : MonoBehaviour
{
    private TextMeshProUGUI localeText;

    void Start()
    {
        localeText = GetComponent<TextMeshProUGUI>();
        DisplayLocaleInfo();
    }

    void DisplayLocaleInfo()
    {
        // Get the system's current culture info
        CultureInfo currentCulture = CultureInfo.CurrentCulture;

        string language = currentCulture.TwoLetterISOLanguageName; // e.g., "en"
        string region = currentCulture.Name; // e.g., "en-US"

        localeText.text = $"Location: {language}\nRegion: {region}";
    }
}