using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))] // Ensures an AudioSource is attached
public class LaserEffect : MonoBehaviour
{
    private LineRenderer line;
    private AudioSource audioSource;

    [SerializeField] private Material[] laserMaterials;
    [SerializeField] private AudioClip fireSound; // Assign your sound effect here
    private int currentMaterialIndex = 0;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();

        line.enabled = false;

        if (laserMaterials.Length > 0)
        {
            line.material = laserMaterials[currentMaterialIndex];
        }
    }

    public void FireLaser(Vector3 startPoint, Vector3 endPoint, float duration = 0.2f)
    {
        StopAllCoroutines();
        StartCoroutine(LaserRoutine(startPoint, endPoint, duration));

        // Play the sound effect
        if (fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
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
        line.material = laserMaterials[currentMaterialIndex];
    }
}