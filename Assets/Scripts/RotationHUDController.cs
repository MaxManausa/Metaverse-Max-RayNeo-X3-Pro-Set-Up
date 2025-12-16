using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls a HUD element, mirroring the head/camera rotation onto a 3D gizmo 
/// object and updating the numerical display of the X, Y, Z Euler angles.
/// </summary>
public class RotationHUDController_3D : MonoBehaviour
{
    // --- Configuration ---
    [Header("Rotation Source")]
    [Tooltip("The Transform (e.g., Camera/Head) to track. If null, data must be provided externally via SetExternalRotation.")]
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
        if (rotationXText == null || rotationYText == null || rotationZText == null)
        {
            Debug.LogError("Rotation Text UI references are required. Please assign them in the Inspector.");
            // Script can still run for the 3D gizmo, but numerical display will fail.
        }
    }

    void Update()
    {
        Quaternion rotationToApply;
        Vector3 eulerAnglesToDisplay;

        if (targetTransform != null)
        {
            // MODE 1: Direct Tracking (Local Camera Rotation)
            // Use localRotation (Quaternion) for smooth, gimbal-lock-free rotation application
            rotationToApply = targetTransform.localRotation;
            // Use localEulerAngles for a consistent 0-360 degree display
            eulerAnglesToDisplay = targetTransform.localEulerAngles;
        }
        else
        {
            // MODE 2: External Data Tracking
            rotationToApply = externalRotation;
            eulerAnglesToDisplay = externalRotation.eulerAngles;
        }

        // 1. Apply the full Quaternion rotation to the 3D gizmo object
        // This makes the 3D object mirror the head's rotation in its local space.
        gizmo3DRoot.localRotation = rotationToApply;

        // 2. Update the numerical display
        UpdateNumericalDisplay(eulerAnglesToDisplay);
    }

    /// <summary>
    /// Public method to feed new rotation data into the HUD from an external source (e.g., Network).
    /// </summary>
    /// <param name="newRotation">The incoming Quaternion rotation data.</param>
    public void SetExternalRotation(Quaternion newRotation)
    {
        externalRotation = newRotation;
    }

    /// <summary>
    /// Updates the text elements with the current rotation values (X, Y, Z).
    /// </summary>
    private void UpdateNumericalDisplay(Vector3 rotation)
    {
        // Only update if the references are assigned to prevent errors
        if (rotationXText != null)
            rotationXText.text = $"X: {rotation.x.ToString(FormatString)}°";

        if (rotationYText != null)
            rotationYText.text = $"Y: {rotation.y.ToString(FormatString)}°";

        if (rotationZText != null)
            rotationZText.text = $"Z: {rotation.z.ToString(FormatString)}°";
    }
}