using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AppRunCounter : MonoBehaviour
{
    private TextMeshProUGUI counterText;
    private const string RUN_COUNT_KEY = "TotalAppRuns";

    void Start()
    {
        counterText = GetComponent<TextMeshProUGUI>();
        int runs = PlayerPrefs.GetInt(RUN_COUNT_KEY, 0) + 1;
        PlayerPrefs.SetInt(RUN_COUNT_KEY, runs);
        PlayerPrefs.Save(); // Ensure it saves immediately

        counterText.text = $"Total App Launches: {runs}";
    }
}
