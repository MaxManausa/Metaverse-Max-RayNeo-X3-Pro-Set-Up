using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    // A list to hold all the Canvases/GameObjects we want to cycle through.
    // Assign these objects in the Unity Inspector.
    public List<GameObject> canvasesToSwitch;

    // The time (in seconds) to wait before switching to the next canvas.
    public float switchDelay = 5f;

    // The index of the currently active canvas in the list.
    private int currentIndex = 0;

    void Start()
    {
        // Start the coroutine that handles the timed switching.
        StartCoroutine(SwitchCanvasesRoutine());

        // Ensure only the first canvas is active at the very start.
        InitializeCanvases();
    }

    /// <summary>
    /// Deactivates all canvases except the first one in the list.
    /// </summary>
    private void InitializeCanvases()
    {
        // Safety check to ensure the list isn't empty
        if (canvasesToSwitch == null || canvasesToSwitch.Count == 0)
        {
            Debug.LogError("CanvasesToSwitch list is empty. Please assign GameObjects in the Inspector.");
            return;
        }

        // Deactivate all canvases initially.
        foreach (GameObject canvas in canvasesToSwitch)
        {
            canvas.SetActive(false);
        }

        // Activate only the first canvas.
        canvasesToSwitch[0].SetActive(true);
        currentIndex = 0; // Set the starting index.
    }

    /// <summary>
    /// Coroutine to handle the timed canvas switching.
    /// </summary>
    IEnumerator SwitchCanvasesRoutine()
    {
        // This loop will run indefinitely until the GameObject is destroyed.
        while (true)
        {
            // Wait for the specified delay.
            yield return new WaitForSeconds(switchDelay);

            // Safety check again.
            if (canvasesToSwitch.Count == 0)
            {
                yield break; // Stop the coroutine if the list is empty.
            }

            // 1. Make the current object inactive.
            canvasesToSwitch[currentIndex].SetActive(false);

            // 2. Calculate the index of the next object.
            // Use the modulo operator (%) to loop back to 0 when we reach the end of the list.
            currentIndex = (currentIndex + 1) % canvasesToSwitch.Count;

            // 3. Make the next object active.
            canvasesToSwitch[currentIndex].SetActive(true);
        }
    }

    // The Update method is left empty as the timed logic is handled by the coroutine.
    void Update()
    {

    }
}