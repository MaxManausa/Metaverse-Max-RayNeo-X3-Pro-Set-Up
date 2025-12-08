using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class RealTimeClock : MonoBehaviour
{
    private TextMeshProUGUI clockText;

    void Start()
    {
        clockText = GetComponent<TextMeshProUGUI>();
        StartCoroutine(UpdateTimeRoutine());
    }

    IEnumerator UpdateTimeRoutine()
    {
        while (true)
        {
            DateTime now = DateTime.Now;

            // --- Change the format string here ---
            // The "\n" character forces a new line in the text display.
            // Result will look like:
            // Dec 07, 2025
            // 09:07 PM
            string formatString = "MMM dd, yyyy\nhh:mm tt";

            clockText.text = now.ToString(formatString);
            // ------------------------------------

            yield return new WaitForSeconds(0.2f);
        }
    }
}