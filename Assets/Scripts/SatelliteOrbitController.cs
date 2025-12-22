using UnityEngine;
using System.Collections;
using TMPro; // Required for TextMesh Pro

public class SatelliteOrbitController : MonoBehaviour
{
    [Header("UI Telemetry")]
    public TextMeshProUGUI telemetryText;

    [Header("Movement Timing (Seconds)")]
    public float waitInterval = 15f;    // x
    public float moveDuration = 15f;    // y
    public float zDelta = 200f;         // z shift amount

    [Header("Logic Constraints")]
    public int minRepeats = 2;          // w
    [Range(0, 100)] public float repeatChance = 50f; // v%
    public float upperLimit = 2000f;
    public float lowerLimit = 200f;

    // Conversion: 1000 Z units = 30,000 Miles (1 unit = 30 Miles)
    private const float MILES_PER_UNIT = 30f;
    private int direction = 1; // 1 for up, -1 for down
    private bool isManuallyOverridden = false;

    void Start()
    {
        // 3. Start at position z 1000
        SetZPosition(1000f);

        // Ensure UI is updated immediately on start
        UpdateTelemetryDisplay();

        StartCoroutine(OrbitRoutine());
    }

    void Update()
    {
        // Continuous update to catch any external transform changes
        UpdateTelemetryDisplay();
    }

    // 2. Public method to set Z manually from other scripts
    public void SetZPosition(float newZ)
    {
        Vector3 pos = transform.position;
        pos.z = newZ;
        transform.position = pos;

        // Force UI update so it doesn't wait for next frame
        UpdateTelemetryDisplay();

        isManuallyOverridden = true;
        CancelInvoke(nameof(ResumeAuto));
        Invoke(nameof(ResumeAuto), 1f);
    }


    private void ResumeAuto() => isManuallyOverridden = false;

    private void UpdateTelemetryDisplay()
    {
        if (telemetryText == null) return;

        float currentMiles = transform.position.z * MILES_PER_UNIT;

        // Formats as: "Altitude: 30,000 mi" 
        // N0 adds the comma separator for thousands
        telemetryText.text = $"Altitude: {currentMiles:N0} mi";
    }

    public void RestartOrbit()
    {
        StopAllCoroutines(); // Kill any stuck or frozen timers
        isManuallyOverridden = false; // Reset the manual lock
        StartCoroutine(OrbitRoutine()); // Start fresh
    }

    IEnumerator OrbitRoutine()
    {
        while (true)
        {
            // Wait for x seconds before starting a movement cycle
            yield return new WaitForSeconds(waitInterval);

            if (isManuallyOverridden) continue;

            int repeatCount = 0;
            bool seriesActive = true;

            while (seriesActive)
            {
                // Rubber Band Logic: If at limits, force direction back toward center (1000)
                if (transform.position.z >= upperLimit) direction = -1;
                else if (transform.position.z <= lowerLimit) direction = 1;

                float startZ = transform.position.z;
                float targetZ = startZ + (zDelta * direction);
                float elapsed = 0;

                // 1. Move slowly over y seconds
                while (elapsed < moveDuration)
                {
                    // Safety: Break if a manual position is set during movement
                    if (isManuallyOverridden) break;

                    elapsed += Time.deltaTime;
                    float currentZ = Mathf.Lerp(startZ, targetZ, elapsed / moveDuration);

                    Vector3 p = transform.position;
                    p.z = currentZ;
                    transform.position = p;

                    yield return null;
                }

                repeatCount++;

                // Logic: Move at least w times before calculating v% chance to repeat
                if (repeatCount >= minRepeats)
                {
                    if (Random.Range(0f, 100f) > repeatChance)
                    {
                        seriesActive = false;
                        // Swap direction for the next big interval cycle
                        direction *= -1;
                    }
                }

                // If we've hit a boundary during a repeat, stop the series and head back
                if (transform.position.z >= upperLimit || transform.position.z <= lowerLimit)
                {
                    seriesActive = false;
                }
            }
        }
    }
}