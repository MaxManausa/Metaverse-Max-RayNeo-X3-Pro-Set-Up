using RayNeo;
using System.Collections.Generic; // Required for List<T>
using UnityEngine;

public class TouchEvents : MonoBehaviour
{
    // Changed individual SerializedFields to a list.
    // Populate this list in the Inspector by dragging and dropping your GameObjects.
    [SerializeField] SceneManagerButtons sceneManagerButtons;
    [SerializeField] VideoPlaylistController videoPlayerController;
    [SerializeField]
    private List<GameObject> displayObjects = new List<GameObject>();

    // Tracks the current index in the list.
    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        SimpleTouchForLite.Instance.OnSwipeUp.AddListener(OnSwipeUp);
        SimpleTouchForLite.Instance.OnSwipeDown.AddListener(OnSwipeDown);
        SimpleTouchForLite.Instance.OnSwipeLeft.AddListener(OnSwipeLeft);
        SimpleTouchForLite.Instance.OnSwipeRight.AddListener(OnSwipeRight);

        SimpleTouchForLite.Instance.OnTripleTap.AddListener(OnTripleTap);
        SimpleTouchForLite.Instance.OnLongPress.AddListener(OnLongPress);
    }



    private void OnDestroy()
    {
        SimpleTouchForLite.Instance.OnSwipeUp.RemoveListener(OnSwipeUp);
        SimpleTouchForLite.Instance.OnSwipeDown.RemoveListener(OnSwipeDown);
        SimpleTouchForLite.Instance.OnSwipeLeft.RemoveListener(OnSwipeLeft);
        SimpleTouchForLite.Instance.OnSwipeRight.RemoveListener(OnSwipeRight);

        SimpleTouchForLite.Instance.OnTripleTap.RemoveListener(OnTripleTap);
        SimpleTouchForLite.Instance.OnLongPress.RemoveListener(OnLongPress);
    }


    /// <summary>
    /// Handles swiping right, moving to the next item in the list.
    /// </summary>
    private void OnSwipeRight(Vector2 pos)
    {
        // Check if there are items to cycle through
        if (displayObjects.Count < 2) return;

        // Deactivate the current object
        if (displayObjects[currentIndex] != null)
            displayObjects[currentIndex].SetActive(false);

        // Move to the next index, wrapping back to 0 if it goes past the end
        currentIndex = (currentIndex + 1) % displayObjects.Count;

        // Activate the new current object
        if (displayObjects[currentIndex] != null)
            displayObjects[currentIndex].SetActive(true);

        Debug.Log($"Moved Right. Current index: {currentIndex}");
    }

    /// <summary>
    /// Handles swiping left, moving to the previous item in the list.
    /// </summary>
    private void OnSwipeLeft(Vector2 pos)
    {
        // Check if there are items to cycle through
        if (displayObjects.Count < 2) return;

        // Deactivate the current object
        if (displayObjects[currentIndex] != null)
            displayObjects[currentIndex].SetActive(false);

        // Move to the previous index, wrapping to the last element if it goes below 0
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = displayObjects.Count - 1;
        }

        // Activate the new current object
        if (displayObjects[currentIndex] != null)
            displayObjects[currentIndex].SetActive(true);

        Debug.Log($"Moved Left. Current index: {currentIndex}");
    }

    private void OnSwipeDown(Vector2 pos)
    {
        if (currentIndex == 1)
        {
            videoPlayerController.GoBack();
            Debug.Log("Play Previous Song");
        }
    }

    private void OnSwipeUp(Vector2 pos)
    {
        if (currentIndex == 1)
        {
            videoPlayerController.PlayNext();
            Debug.Log("Play Next Song");
        }
    }

    private void OnTripleTap()
    {
        // 1. Deactivate the object that was visible before the triple tap.
        // This is the object at the old currentIndex.
        if (displayObjects.Count > 0 && displayObjects[currentIndex] != null)
        {
            displayObjects[currentIndex].SetActive(false);
        }

        // 2. Set the index to 0. This is the desired reset point.
        currentIndex = 0;

        // 3. Activate the new current object (the one at index 0).
        if (displayObjects.Count > 0 && displayObjects[currentIndex] != null)
        {
            displayObjects[currentIndex].SetActive(true);
        }

        // 4. Call the Scene Manager action.
        sceneManagerButtons.Go0DOF();

        // Optional: Log the reset
        Debug.Log($"Triple Tap: Resetting display to index {currentIndex}.");
    }

    private void OnLongPress()
    {

    }
}
