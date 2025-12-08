using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimulatedPedometer : MonoBehaviour
{
    private TextMeshProUGUI stepText;
    private int stepCount = 0;
    private Vector3 acceleration;
    private float shakeThreshold = 2.0f; // Threshold to register a 'step'

    void Start()
    {
        stepText = GetComponent<TextMeshProUGUI>();
        stepText.text = "Steps: 0";
    }

    void Update()
    {
        // Simple shake detection as a proxy for movement/steps
        acceleration = Input.acceleration;

        if (acceleration.sqrMagnitude >= shakeThreshold * shakeThreshold)
        {
            // Update the count every few frames where movement is detected
            if (Time.frameCount % 10 == 0)
            {
                stepCount++;
                stepText.text = $"Steps: {stepCount}";
            }
        }
    }
}
