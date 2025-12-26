using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
public class LaserEffect : MonoBehaviour
{
    private LineRenderer line;
    private AudioSource audioSource;

    [SerializeField] private Material[] laserMaterials;
    [SerializeField] private AudioClip fireSound;
    private int currentMaterialIndex = 0;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();

        line.enabled = false;
        line.positionCount = 2;

        // Ensures the laser is visible through other objects
        line.sortingOrder = 10;

        if (laserMaterials.Length > 0)
        {
            line.material = laserMaterials[currentMaterialIndex];
        }
    }

    public void FireLaser(Vector3 startPoint, Vector3 endPoint, float duration = 0.2f)
    {
        if (line == null) return;

        StopAllCoroutines();
        StartCoroutine(LaserRoutine(startPoint, endPoint, duration));

        if (fireSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
    }

    private IEnumerator LaserRoutine(Vector3 start, Vector3 end, float time)
    {
        line.enabled = true;
        line.SetPosition(0, start);
        line.SetPosition(1, end);

        // Debug line visible only in the Scene View to verify the logic works
        Debug.DrawLine(start, end, Color.red, time);

        yield return new WaitForSeconds(time);

        line.enabled = false;
    }

    public void NextColor()
    {
        if (laserMaterials.Length == 0) return;
        currentMaterialIndex = (currentMaterialIndex + 1) % laserMaterials.Length;
        UpdateLaserMaterial();
    }

    public void PreviousColor()
    {
        if (laserMaterials.Length == 0) return;
        currentMaterialIndex--;
        if (currentMaterialIndex < 0) currentMaterialIndex = laserMaterials.Length - 1;
        UpdateLaserMaterial();
    }

    private void UpdateLaserMaterial()
    {
        if (line != null && laserMaterials.Length > 0)
            line.material = laserMaterials[currentMaterialIndex];
    }
}