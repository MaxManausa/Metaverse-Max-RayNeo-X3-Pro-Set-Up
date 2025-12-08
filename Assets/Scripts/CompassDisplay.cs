using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompassDisplay : MonoBehaviour
{
    private TextMeshProUGUI compassText;

    IEnumerator Start()
    {
        compassText = GetComponent<TextMeshProUGUI>();

        // Enable compass and location services for accurate heading
        Input.location.Start();
        Input.compass.enabled = true;

        // Wait for services to initialize
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1 || Input.location.status == LocationServiceStatus.Failed)
        {
            compassText.text = "Compass Unavailable";
            yield break;
        }

        // Start updating the heading regularly
        InvokeRepeating("UpdateHeading", 1f, 0.5f);
    }

    void UpdateHeading()
    {
        // Get the current compass heading (degrees)
        float heading = Input.compass.trueHeading;

        // Format it nicely
        compassText.text = $"HDG: {Mathf.RoundToInt(heading)}°";
    }

    void OnDisable()
    {
        // Remember to stop services when done
        Input.location.Stop();
        Input.compass.enabled = false;
    }
}