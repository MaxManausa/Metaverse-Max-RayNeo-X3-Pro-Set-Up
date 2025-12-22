using RayNeo;
using System.Collections.Generic; // Required for List<T>
using UnityEngine;

public class SEDTouchEvents : MonoBehaviour
{
    [SerializeField] private SEDSceneManager sceneManager;
    [SerializeField] private LaserEffect laserColor;

    void Start()
    {
        // Subscribe to RayNeo touch events
        var touch = SimpleTouchForLite.Instance;
        touch.OnSwipeUp.AddListener(OnSwipeUp);
        touch.OnSwipeDown.AddListener(OnSwipeDown);
        touch.OnSwipeLeft.AddListener(OnSwipeLeft);
        touch.OnSwipeRight.AddListener(OnSwipeRight);
        touch.OnSimpleTap.AddListener(OnSimpleTap);
        touch.OnTripleTap.AddListener(OnTripleTap);
        touch.OnLongPress.AddListener(OnLongPress);
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks or null reference errors
        if (SimpleTouchForLite.Instance == null) return;

        var touch = SimpleTouchForLite.Instance;
        touch.OnSwipeUp.RemoveListener(OnSwipeUp);
        touch.OnSwipeDown.RemoveListener(OnSwipeDown);
        touch.OnSwipeLeft.RemoveListener(OnSwipeLeft);
        touch.OnSwipeRight.RemoveListener(OnSwipeRight);
        touch.OnSimpleTap.RemoveListener(OnSimpleTap);
        touch.OnTripleTap.RemoveListener(OnTripleTap);
        touch.OnLongPress.RemoveListener(OnLongPress);
    }

    private void OnSwipeRight(Vector2 pos) => laserColor.NextColor();
    private void OnSwipeLeft(Vector2 pos) => laserColor.PreviousColor();
    private void OnSwipeDown(Vector2 pos) { /* Logic if needed */ }
    private void OnSwipeUp(Vector2 pos) { /* Logic if needed */ }

    private void OnSimpleTap()
    {
        // 1. If currently playing, tap to Pause
        if (sceneManager.gamePlaying)
        {
            sceneManager.PauseGame();
            return;
        }

        // 2. If currently paused, tap to Continue
        if (sceneManager.gamePaused)
        {
            sceneManager.ContinueGame();
            return;
        }

        // 3. Handle Game Over State (Won vs Failed)
        if (sceneManager.gameOver)
        {
            if (sceneManager.wonLevel)
            {
                // Won Screen -> Go to next level
                sceneManager.NextLevel();
            }
            else
            {
                // Fail Screen -> Go back to Home Screen
                sceneManager.GoHome();
            }
            return;
        }

        // 4. Fallback: If at Home, tap to Start
        sceneManager.StartGame();
    }

    private void OnTripleTap()
    {
        // If we are at the Home Screen
        if (!sceneManager.gamePlaying && !sceneManager.gamePaused)
        {
            Application.Quit();
            return;
        }

        // If in game or paused, go back to main menu
        sceneManager.GoHome();
    }

    private void OnLongPress() { /* Logic if needed */ }
}