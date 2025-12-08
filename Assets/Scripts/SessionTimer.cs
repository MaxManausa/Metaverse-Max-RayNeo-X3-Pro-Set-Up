using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SessionTimer : MonoBehaviour
{
    private TextMeshProUGUI timerText;

    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Time.time is the time since the application started
        TimeSpan t = TimeSpan.FromSeconds(Time.time);

        // Format the time as Minutes:Seconds
        timerText.text = string.Format("Session Time: {0:D2}m:{1:D2}s",
            t.Minutes,
            t.Seconds);
    }
}
