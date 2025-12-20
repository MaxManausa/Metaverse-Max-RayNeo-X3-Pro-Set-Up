using RayNeo;
using System.Collections.Generic; // Required for List<T>
using UnityEngine;

public class SEDTouchEvents : MonoBehaviour
{
    [SerializeField] private SEDSceneManager sceneManager;
    [SerializeField] private LaserEffect laserColor;
 
    // Start is called before the first frame update
    void Start()
    {
        SimpleTouchForLite.Instance.OnSwipeUp.AddListener(OnSwipeUp);
        SimpleTouchForLite.Instance.OnSwipeDown.AddListener(OnSwipeDown);
        SimpleTouchForLite.Instance.OnSwipeLeft.AddListener(OnSwipeLeft);
        SimpleTouchForLite.Instance.OnSwipeRight.AddListener(OnSwipeRight);

        SimpleTouchForLite.Instance.OnSimpleTap.AddListener(OnSimpleTap);
        SimpleTouchForLite.Instance.OnTripleTap.AddListener(OnTripleTap);
        SimpleTouchForLite.Instance.OnLongPress.AddListener(OnLongPress);
    }



    private void OnDestroy()
    {
        SimpleTouchForLite.Instance.OnSwipeUp.RemoveListener(OnSwipeUp);
        SimpleTouchForLite.Instance.OnSwipeDown.RemoveListener(OnSwipeDown);
        SimpleTouchForLite.Instance.OnSwipeLeft.RemoveListener(OnSwipeLeft);
        SimpleTouchForLite.Instance.OnSwipeRight.RemoveListener(OnSwipeRight);

        SimpleTouchForLite.Instance.OnSimpleTap.RemoveListener(OnSimpleTap);
        SimpleTouchForLite.Instance.OnTripleTap.RemoveListener(OnTripleTap);
        SimpleTouchForLite.Instance.OnLongPress.RemoveListener(OnLongPress);
    }


    /// <summary>
    /// Handles swiping right, moving to the next item in the list.
    /// </summary>
    private void OnSwipeRight(Vector2 pos)
    {
        Debug.Log("Moved Right.");
        laserColor.NextColor();
    }

    /// <summary>
    /// Handles swiping left, moving to the previous item in the list.
    /// </summary>
    private void OnSwipeLeft(Vector2 pos)
    {
        Debug.Log("Moved Left.");
        laserColor.PreviousColor();
    }

    private void OnSwipeDown(Vector2 pos)
    {
        Debug.Log("Swiped Down");
    }

    private void OnSwipeUp(Vector2 pos)
    {
        Debug.Log("Swiped Up");
    }

    private void OnSimpleTap()
    {
        Debug.Log("Tapped Once");
        if (sceneManager.gamePlaying == true)
        {
            sceneManager.PauseGame();
            return;
        }
        else if (sceneManager.gamePaused == true)
        {
            sceneManager.ContinueGame();
            return;
        }
        else if (sceneManager.gamePaused == false && sceneManager.gamePlaying == false)
        {
            sceneManager.StartGame();
        }
    }


    private void OnTripleTap()
    {
        sceneManager.GoHome();
        Debug.Log("Triple Tapped");
    }

    private void OnLongPress()
    {
        Debug.Log("Long Pressed");
    }
}
