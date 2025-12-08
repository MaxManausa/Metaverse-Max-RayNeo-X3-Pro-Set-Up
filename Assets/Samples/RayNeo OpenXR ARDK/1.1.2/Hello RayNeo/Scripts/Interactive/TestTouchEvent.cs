using RayNeo;
using UnityEngine;
using UnityEngine.UI;

public class TestTouchEvent : MonoBehaviour
{
    public Button UpButton;
    public Button DownButton;
    public Button LeftButton;
    public Button RightButton;
    public Button OnTripleTapButton;
    public Button OnLongPressButton;

    // Start is called before the first frame update
    void Start()
    {
        SimpleTouchForLite.Instance.OnSwipeUp.AddListener(OnSwipeUp);
        SimpleTouchForLite.Instance.OnSwipeDown.AddListener(OnSwipeDown);
        SimpleTouchForLite.Instance.OnSwipeLeft.AddListener(OnSwipeLeft);
        SimpleTouchForLite.Instance.OnSwipeRight.AddListener(OnSwipeRight);

        SimpleTouchForLite.Instance.OnTripleTap.AddListener(OnTripleTapButtonImageRandomColor);
        SimpleTouchForLite.Instance.OnLongPress.AddListener(OnLongPress);
    }

    

    private void OnDestroy()
    {
        SimpleTouchForLite.Instance.OnSwipeUp.RemoveListener(OnSwipeUp);
        SimpleTouchForLite.Instance.OnSwipeDown.RemoveListener(OnSwipeDown);
        SimpleTouchForLite.Instance.OnSwipeLeft.RemoveListener(OnSwipeLeft);
        SimpleTouchForLite.Instance.OnSwipeRight.RemoveListener(OnSwipeRight);

        SimpleTouchForLite.Instance.OnTripleTap.RemoveListener(OnTripleTapButtonImageRandomColor);
        SimpleTouchForLite.Instance.OnLongPress.RemoveListener(OnLongPress);
    }


    private void OnSwipeRight(Vector2 pos)
    {
        RightButton.image.color = Color.green;
        LeftButton.image.color = Color.white;
    }

    private void OnSwipeLeft(Vector2 pos)
    {
        RightButton.image.color = Color.white;
        LeftButton.image.color = Color.green;
    }

    private void OnSwipeDown(Vector2 pos)
    {
        UpButton.image.color = Color.white;
        DownButton.image.color = Color.green;
    }

    private void OnSwipeUp(Vector2 pos)
    {
        UpButton.image.color = Color.green;
        DownButton.image.color = Color.white;
    }

    private void OnTripleTapButtonImageRandomColor()
    {
        OnTripleTapButton.image.color = new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f));
    }

    private void OnLongPress()
    {
        OnLongPressButton.image.color = new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f));
    }
}
