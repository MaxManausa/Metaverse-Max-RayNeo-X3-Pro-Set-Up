using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserEffect : MonoBehaviour
{
    private LineRenderer line;
    [SerializeField] private Material[] laserMaterials; // Assign your materials here in the Inspector
    private int currentMaterialIndex = 0;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;

        // Initialize with the first material if the list isn't empty
        if (laserMaterials.Length > 0)
        {
            line.material = laserMaterials[currentMaterialIndex];
        }
    }

    public void FireLaser(Vector3 startPoint, Vector3 endPoint, float duration = 0.2f)
    {
        StopAllCoroutines();
        StartCoroutine(LaserRoutine(startPoint, endPoint, duration));
    }

    private IEnumerator LaserRoutine(Vector3 start, Vector3 end, float time)
    {
        line.enabled = true;
        line.SetPosition(0, start);
        line.SetPosition(1, end);

        yield return new WaitForSeconds(time);

        line.enabled = false;
    }

    public void NextColor()
    {
        if (laserMaterials.Length == 0) return;

        // Increment index and wrap around using Modulo
        currentMaterialIndex = (currentMaterialIndex + 1) % laserMaterials.Length;
        UpdateLaserMaterial();
    }

    public void PreviousColor()
    {
        if (laserMaterials.Length == 0) return;

        // Decrement index and wrap to end if below zero
        currentMaterialIndex--;
        if (currentMaterialIndex < 0)
        {
            currentMaterialIndex = laserMaterials.Length - 1;
        }
        UpdateLaserMaterial();
    }

    private void UpdateLaserMaterial()
    {
        line.material = laserMaterials[currentMaterialIndex];
    }
}