using RayNeo;
using UnityEngine;
using UnityEngine.UI; // Essential for standard Text component

public class ObjectSwitcher : MonoBehaviour
{
    [Header("UI Display")]
    [SerializeField] private Text counterText; // Standard UI Text slot

    private int _currentIndex = 0;
    private int _childCount;

    void Start()
    {
        // Get the total number of children under this parent
        _childCount = transform.childCount;

        if (_childCount == 0)
        {
            Debug.LogWarning("TestTouchEvent: No child objects found!");
            return;
        }

        // Initialize display and state
        UpdateActiveObject();

        // Subscribe to RayNeo Touch Events
        SimpleTouchForLite.Instance.OnSwipeLeft.AddListener(OnSwipeLeft);
        SimpleTouchForLite.Instance.OnSwipeRight.AddListener(OnSwipeRight);
    }

    private void OnDestroy()
    {
        if (SimpleTouchForLite.Instance != null)
        {
            SimpleTouchForLite.Instance.OnSwipeLeft.RemoveListener(OnSwipeLeft);
            SimpleTouchForLite.Instance.OnSwipeRight.RemoveListener(OnSwipeRight);
        }
    }

    private void OnSwipeRight(Vector2 pos)
    {
        _currentIndex++;
        if (_currentIndex >= _childCount) _currentIndex = 0;
        UpdateActiveObject();
    }

    private void OnSwipeLeft(Vector2 pos)
    {
        _currentIndex--;
        if (_currentIndex < 0) _currentIndex = _childCount - 1;
        UpdateActiveObject();
    }

    private void UpdateActiveObject()
    {
        // Loop through all children and enable only the current one
        for (int i = 0; i < _childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == _currentIndex);
        }

        // Update the UI Text with the number "n"
        UpdateCounterUI();
    }

    private void UpdateCounterUI()
    {
        if (counterText != null)
        {
            // n is the human-readable number (starts at 1)
            int n = _currentIndex + 1;
            counterText.text = n.ToString();
        }
    }
}