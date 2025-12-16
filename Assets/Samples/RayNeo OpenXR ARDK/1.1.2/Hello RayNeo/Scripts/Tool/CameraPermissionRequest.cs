using UnityEngine;
using System.Collections;
using UnityEngine.Android; // Crucial for accessing the Permission API

public class CameraPermissionRequest : MonoBehaviour
{
    [Header("Permissions to Request")]
    [Tooltip("Should the script attempt to get Location (GPS) permission? This is needed for world-tracking.")]
    public bool requestLocation = true;

    // Define the string constants for the permissions we need
    private string CameraPermission = Permission.Camera;
    private string FineLocationPermission = Permission.FineLocation;

    void Start()
    {
        // Start the permission request process asynchronously
        StartCoroutine(RequestAllPermissions());
    }

    private IEnumerator RequestAllPermissions()
    {
        // --- 1. Request Camera Permission ---

        if (!Permission.HasUserAuthorizedPermission(CameraPermission))
        {
            Debug.Log("Requesting Camera Permission...");
            Permission.RequestUserPermission(CameraPermission);

            // Wait for a short time for the user to respond to the prompt
            yield return null;
            yield return new WaitForSeconds(0.1f); // Wait a fraction of a second
        }

        if (Permission.HasUserAuthorizedPermission(CameraPermission))
        {
            Debug.Log("Camera Permission Granted.");
        }
        else
        {
            Debug.LogError("Camera Permission Denied. Head tracking functionality may be limited.");
        }

        // --- 2. Request Location Permission (if enabled) ---

        if (requestLocation)
        {
            if (!Permission.HasUserAuthorizedPermission(FineLocationPermission))
            {
                Debug.Log("Requesting Fine Location Permission...");
                Permission.RequestUserPermission(FineLocationPermission);

                // Wait for the user to respond
                yield return null;
                yield return new WaitForSeconds(0.1f);
            }

            if (Permission.HasUserAuthorizedPermission(FineLocationPermission))
            {
                Debug.Log("Fine Location Permission Granted.");
            }
            else
            {
                Debug.LogWarning("Fine Location Permission Denied. GPS and world-tracking features may not work.");
            }
        }

        Debug.Log("All requested permissions handled.");
    }
}