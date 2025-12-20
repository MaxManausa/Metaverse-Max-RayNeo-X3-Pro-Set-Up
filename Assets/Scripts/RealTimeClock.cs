using UnityEngine;
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

            // ddMMM: Day and 3-letter Month
            // yy: Short year
            // .HH:mm: Period separator and 24-hour time
            string timestamp = now.ToString("ddMMMyy.HH:mm").ToUpper();

            clockText.text = timestamp;

            // Wait for 1 second to stay precise to the minute
            yield return new WaitForSeconds(1.0f);
        }
    }
}