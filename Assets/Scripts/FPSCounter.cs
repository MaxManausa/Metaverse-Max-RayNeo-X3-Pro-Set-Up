using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    private TextMeshProUGUI fpsText;
    public float updateInterval = 0.5f;
    private float accum = 0;
    private int frames = 0;
    private float timeleft;

    void Start()
    {
        // Ensure you attach this script to the TextMeshPro UI object
        fpsText = GetComponent<TextMeshProUGUI>();
        timeleft = updateInterval;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text
        if (timeleft <= 0.0)
        {
            float fps = accum / frames;
            // Use color coding for readability: Red for poor performance, Green for good
            string color = (fps < 30) ? "<color=red>" : (fps < 50 ? "<color=yellow>" : "<color=green>");
            fpsText.text = color + Mathf.RoundToInt(fps) + " FPS</color>";

            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
    }
}
