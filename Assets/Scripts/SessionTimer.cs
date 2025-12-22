using UnityEngine;
using TMPro;
using System;

public class SessionTimer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private float timeElapsed;

    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    // This triggers whenever the object this script is attached to becomes active
    // Or, if this script is on a manager, you can call ResetTimer() from your Home button
    void OnEnable()
    {
        ResetTimer();
    }

    public void ResetTimer()
    {
        timeElapsed = 0f;
    }

    void Update()
    {
        if (timerText == null) return;

        // Increase our custom timer by the time passed since the last frame
        timeElapsed += Time.deltaTime;

        TimeSpan t = TimeSpan.FromSeconds(timeElapsed);

        // Format: MET Hours:Minutes:Seconds
        timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
            (int)t.TotalHours,
            t.Minutes,
            t.Seconds);
    }
}