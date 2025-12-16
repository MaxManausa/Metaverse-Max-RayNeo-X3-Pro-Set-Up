using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls a HUD element, mirroring the head/camera rotation onto a 3D gizmo 
/// object (like a face-to-face mirror) and updating the numerical display 
/// with the original, unmirrored rotation values.
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
        // Check for required components
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
            // Use localEulerAngles for a consistent 0-360 degree display
            sourceEuler = targetTransform.localEulerAngles;
        }
        else
        {
            // Use externalRotation's Euler angles
            sourceEuler = externalRotation.eulerAngles;
        }

        // 2. MIRROR THE ROTATION: Negate all three Euler angles for the 3D gizmo.

        // A. Convert the 0-360 range to a signed -180 to 180 range for reliable negation.
        Vector3 signedSourceEuler = ConvertToSignedAngles(sourceEuler);

        // B. Negate all three components (X, Y, and Z) for the mirror effect.
        Vector3 invertedEuler = new Vector3(
            -signedSourceEuler.x,
            -signedSourceEuler.y,
            -signedSourceEuler.z
        );

        // C. Apply the mirrored rotation to the 3D gizmo object
        Quaternion mirroredRotation = Quaternion.Euler(invertedEuler);
        gizmo3DRoot.localRotation = mirroredRotation;

        // 3. Update the numerical display (using the ORIGINAL, unmirrored source angles)
        UpdateNumericalDisplay(sourceEuler);
    }

    /// <summary>
    /// Converts Euler angles from 0-360 range to a signed -180 to 180 range.
    /// Necessary for reliable mathematical negation (mirroring).
    /// </summary>
    private Vector3 ConvertToSignedAngles(Vector3 euler)
    {
        // If angle > 180, subtract 360 to get a negative value
        euler.x = (euler.x > 180f) ? euler.x - 360f : euler.x;
        euler.y = (euler.y > 180f) ? euler.y - 360f : euler.y;
        euler.z = (euler.z > 180f) ? euler.z - 360f : euler.z;
        return euler;
    }

    /// <summary>
    /// Public method to feed new rotation data into the HUD from an external source (e.g., Network).
    /// </summary>
    public void SetExternalRotation(Quaternion newRotation)
    {
        externalRotation = newRotation;
    }

    /// <summary>
    /// Updates the text elements with the current rotation values (X, Y, Z).
    /// </summary>
    private void UpdateNumericalDisplay(Vector3 rotation)
    {
        // Only update if the references are assigned
        if (rotationXText != null)
            rotationXText.text = $"X: {rotation.x.ToString(FormatString)}°";

        if (rotationYText != null)
            rotationYText.text = $"Y: {rotation.y.ToString(FormatString)}°";

        if (rotationZText != null)
            rotationZText.text = $"Z: {rotation.z.ToString(FormatString)}°";
    }
}