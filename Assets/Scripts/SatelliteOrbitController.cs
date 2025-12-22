using UnityEngine;
using System.Collections;
using TMPro; // Required for TextMesh Pro

public class SatelliteOrbitController : MonoBehaviour
{
    [Header("UI Telemetry")]
    public TextMeshProUGUI telemetryText;

    [Header("Movement Timing")]
    public float waitInterval = 15f;    // x
    public float moveDuration = 15f;    // y
    public float zDelta = 200f;         // z shift

    [Header("Logic Constraints")]
    public int minRepeats = 2;          // w
    [Range(0, 100)] public float repeatChance = 50f; // v%
    public float upperLimit = 2000f;
    public float lowerLimit = 200f;

    // Conversion: 1000 Z = 30,000 Miles -> 1 Z = 30 Miles
    private const float MILES_PER_UNIT = 30f;
    private int direction = 1;
    private bool isManuallyOverridden = false;

    void Start()
    {
        // 3. Start at position z 1000
        SetZPosition(1000f);
        StartCoroutine(OrbitRoutine());
    }

    void Update()
    {
        UpdateTelemetryDisplay();
    }

    // 2. Public method to set Z manually
    public void SetZPosition(float newZ)
    {
        Vector3 pos = transform.position;
        pos.z = newZ;
        transform.position = pos;

        isManuallyOverridden = true;
        CancelInvoke("ResumeAuto");
        Invoke("ResumeAuto", 1f);
    }

    private void ResumeAuto() => isManuallyOverridden = false;

    private void UpdateTelemetryDisplay()
    {
        if (telemetryText == null) return;

        float currentMiles = transform.position.z * MILES_PER_UNIT;

        // Clean Military Display: Altitude: 30,000 mi
        // (Uses N0 for comma separators and no prose text)
        telemetryText.text = $"Altitude: {currentMiles:N0} mi";
    }

    IEnumerator OrbitRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitInterval);

            if (isManuallyOverridden) continue;

            int repeatCount = 0;
            bool seriesActive = true;

            while (seriesActive)
            {
                // Rubber Band Logic
                if (transform.position.z >= upperLimit) direction = -1;
                else if (transform.position.z <= lowerLimit) direction = 1;

                float startZ = transform.position.z;
                float targetZ = startZ + (zDelta * direction);
                float elapsed = 0;

                while (elapsed < moveDuration)
                {
                    elapsed += Time.deltaTime;
                    float currentZ = Mathf.Lerp(startZ, targetZ, elapsed / moveDuration);

                    Vector3 p = transform.position;
                    p.z = currentZ;
                    transform.position = p;
                    yield return null;
                }

                repeatCount++;

                if (repeatCount >= minRepeats)
                {
                    if (Random.Range(0f, 100f) > repeatChance)
                    {
                        seriesActive = false;
                        direction *= -1;
                    }
                }

                if (transform.position.z >= upperLimit || transform.position.z <= lowerLimit)
                {
                    seriesActive = false;
                }
            }
        }
    }
}