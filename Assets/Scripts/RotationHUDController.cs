using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls a HUD element, mirroring X and Y rotation for a mirror effect,
/// but keeping Z rotation normal.
/// </summary>
public class RotationHUDController : MonoBehaviour
{
    // --- Configuration ---
    [Header("Rotation Source")]
    [Tooltip("The Transform (e.g., Camera/Head) to track. If null, data must be provided externally.")]
    public Transform targetTransform;

    // --- 3D Gizmo Reference ---
    [Header("3D Gizmo Component")]
    [Tooltip("The single parent GameObject containing the 3D rotation gizmo.")]
    public Transform gizmo3DRoot;

    // --- Numeric Display References (UI) ---
    [Header("Numeric Display")]
    public Text rotationXText;
    public Text rotationYText;
    public Text rotationZText;

    // --- Internal State ---
    private Quaternion externalRotation = Quaternion.identity;
    private const string FormatString = "F1"; // Display format: one decimal place

    void Start()
    {
        if (gizmo3DRoot == null)
        {
            Debug.LogError("3D Gizmo Root Transform is required. Please assign it in the Inspector.");
            enabled = false;
        }
    }

    void Update()
    {
        Vector3 sourceEuler;

        // 1. Determine the source rotation
        if (targetTransform != null)
        {
            sourceEuler = targetTransform.localEulerAngles;
        }
        else
        {
            sourceEuler = externalRotation.eulerAngles;
        }

        // 2. MIRROR THE ROTATION: Modified to flip only X and Y.

        // A. Convert the 0-360 range to signed -180 to 180 range.
        Vector3 signedSourceEuler = ConvertToSignedAngles(sourceEuler);

        // B. Negate X and Y for the mirror effect, but keep Z positive (normal).
        Vector3 invertedEuler = new Vector3(
            -signedSourceEuler.x,
            -signedSourceEuler.y,
            signedSourceEuler.z // Removed the minus sign here
        );

        // C. Apply the rotation to the 3D gizmo object
        Quaternion mirroredRotation = Quaternion.Euler(invertedEuler);
        gizmo3DRoot.localRotation = mirroredRotation;

        // 3. Update numerical display (using the ORIGINAL, unmirrored source angles)
        UpdateNumericalDisplay(sourceEuler);
    }

    private Vector3 ConvertToSignedAngles(Vector3 euler)
    {
        euler.x = (euler.x > 180f) ? euler.x - 360f : euler.x;
        euler.y = (euler.y > 180f) ? euler.y - 360f : euler.y;
        euler.z = (euler.z > 180f) ? euler.z - 360f : euler.z;
        return euler;
    }

    public void SetExternalRotation(Quaternion newRotation)
    {
        externalRotation = newRotation;
    }

    private void UpdateNumericalDisplay(Vector3 rotation)
    {
        if (rotationXText != null)
            rotationXText.text = $"X: {rotation.x.ToString(FormatString)}°";

        if (rotationYText != null)
            rotationYText.text = $"Y: {rotation.y.ToString(FormatString)}°";

        if (rotationZText != null)
            rotationZText.text = $"Z: {rotation.z.ToString(FormatString)}°";
    }
}