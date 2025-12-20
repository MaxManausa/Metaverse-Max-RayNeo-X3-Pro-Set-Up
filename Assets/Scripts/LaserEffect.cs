using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserEffect : MonoBehaviour
{
    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false; // Start hidden
    }

    /// <summary>
    /// Fires a laser from a start point to an end point for a set duration.
    /// </summary>
    public void FireLaser(Vector3 startPoint, Vector3 endPoint, float duration = 0.2f)
    {
        StopAllCoroutines(); // Reset if firing rapidly
        StartCoroutine(LaserRoutine(startPoint, endPoint, duration));
    }

    private System.Collections.IEnumerator LaserRoutine(Vector3 start, Vector3 end, float time)
    {
        line.enabled = true;

        // Update positions (Satellite and Asteroid)
        line.SetPosition(0, start);
        line.SetPosition(1, end);

        yield return new WaitForSeconds(time);

        line.enabled = false;
    }
}
