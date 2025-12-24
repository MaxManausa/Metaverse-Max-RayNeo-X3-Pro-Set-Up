using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class RealTimeClock : MonoBehaviour
{
    private TextMeshProUGUI clockText;

    void Awake()
    {
        clockText = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        // Start the routine whenever the UI becomes active
        StartCoroutine(UpdateTimeRoutine());
    }

    void OnDisable()
    {
        // Stop the routine when inactive to save resources
        StopAllCoroutines();
    }

    IEnumerator UpdateTimeRoutine()
    {
        while (true)
        {
            DateTime now = DateTime.Now;

            // ddMMM: Day and 3-letter Month
            // yy: Short year
            // .HH:mm: Period separator and 24-hour time
            string timestamp = now.ToString("ddMMMyy.HH:mm").ToUpper();

            if (clockText != null)
            {
                clockText.text = timestamp;
            }

            // FIXED: Use WaitForSecondsRealtime so the clock doesn't freeze 
            // when Time.timeScale is 0 (Paused/Game Over).
            yield return new WaitForSecondsRealtime(1.0f);
        }
    }
}