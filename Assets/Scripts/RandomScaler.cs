using UnityEngine;

public partial class RandomScaler : MonoBehaviour
{
    /*
 * ******************************************************************************************
 * HUGE NOTE: THIS IS A JOKE SCRIPT THAT WILL FULL FUCK UP THE VIEW OF THE EARTH 
 * AND COULD MAYBE BE USED AS PSYCHOLOGICAL WARFARE FROM ALIENS.
 * ******************************************************************************************
 */

    [Header("Base Settings")]
    public Vector3 baseScale = Vector3.one;

    [Header("X Axis Settings")]
    [Range(0.1f, 5f)] public float xSpeed = 1.0f;
    [Range(0f, 5f)] public float xVariance = 1.0f;

    [Header("Y Axis Settings")]
    [Range(0.1f, 5f)] public float ySpeed = 1.2f;
    [Range(0f, 5f)] public float yVariance = 1.5f;

    [Header("Z Axis Settings")]
    [Range(0.1f, 5f)] public float zSpeed = 0.8f;
    [Range(0f, 5f)] public float zVariance = 0.5f;

    // Offsets to ensure they don't all start at the exact same point in the "wave"
    private float offsetX, offsetY, offsetZ;

    void Start()
    {
        // Randomize starting phase so the motion isn't perfectly synchronized at the start
        offsetX = Random.Range(0f, 100f);
        offsetY = Random.Range(0f, 100f);
        offsetZ = Random.Range(0f, 100f);
    }

    void Update()
    {
        // Calculate new scale using Sine waves: base + (Sin(Time * Speed) * Amplitude)
        float newX = baseScale.x + Mathf.Sin(Time.time * xSpeed + offsetX) * xVariance;
        float newY = baseScale.y + Mathf.Sin(Time.time * ySpeed + offsetY) * yVariance;
        float newZ = baseScale.z + Mathf.Sin(Time.time * zSpeed + offsetZ) * zVariance;

        transform.localScale = new Vector3(newX, newY, newZ);
    }
}