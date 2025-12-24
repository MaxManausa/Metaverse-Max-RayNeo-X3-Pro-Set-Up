using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class SEDVideoPlayer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject videoHolder;
    [SerializeField] private GameObject videoPlayerObject;
    [SerializeField] private GameObject videoPlayerBackground;
    [SerializeField] private GameObject gameUIScreen;

    [Header("Level Video Clips")]
    [Tooltip("Assign 10 videos here. Element 0 = Level 1, Element 1 = Level 2, etc.")]
    [SerializeField] private VideoClip[] levelClips = new VideoClip[10];

    private VideoPlayer vPlayer;
    private Coroutine disableTimerCoroutine;
    private bool uiWasActiveLastFrame = false; // Track state to prevent constant resetting

    [Header("Settings")]
    public bool disableVideo = false;
    public float delayAfterEnd = 1.0f;

    private void Awake()
    {
        InitializeVideo();
    }

    private void Update()
    {
        if (gameUIScreen == null) return;

        bool uiIsActiveNow = gameUIScreen.activeInHierarchy;

        // Only trigger Reset if the UI just turned ON (Transition from False to True)
        if (uiIsActiveNow && !uiWasActiveLastFrame)
        {
            ResetForNewLevel();
        }

        uiWasActiveLastFrame = uiIsActiveNow;
    }

    private void InitializeVideo()
    {
        // Initial setup
        if (disableVideo && videoHolder != null)
        {
            RestartDisableTimer();
        }

        if (videoPlayerObject != null)
        {
            vPlayer = videoPlayerObject.GetComponent<VideoPlayer>();
            if (vPlayer != null)
            {
                vPlayer.playOnAwake = false;
                vPlayer.loopPointReached += OnVideoFinished;
            }
            videoPlayerObject.SetActive(false);
        }

        if (videoPlayerBackground != null)
        {
            videoPlayerBackground.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
            videoPlayerBackground.SetActive(false);
        }
    }

    private void ResetForNewLevel()
    {
        if (videoHolder != null)
        {
            videoHolder.SetActive(true);

            if (disableVideo)
            {
                RestartDisableTimer();
            }
        }
    }

    private void RestartDisableTimer()
    {
        if (disableTimerCoroutine != null) StopCoroutine(disableTimerCoroutine);
        disableTimerCoroutine = StartCoroutine(DisableHolderRoutine());
    }

    private IEnumerator DisableHolderRoutine()
    {
        yield return new WaitForSeconds(7f);
        if (videoHolder != null)
        {
            videoHolder.SetActive(false);
        }
        disableTimerCoroutine = null;
    }

    // --- Public Methods called by ScoreManager ---
    public void Level1Video() => PlaySequence(0);
    public void Level2Video() => PlaySequence(1);
    public void Level3Video() => PlaySequence(2);
    public void Level4Video() => PlaySequence(3);
    public void Level5Video() => PlaySequence(4);
    public void Level6Video() => PlaySequence(5);
    public void Level7Video() => PlaySequence(6);
    public void Level8Video() => PlaySequence(7);
    public void Level9Video() => PlaySequence(8);
    public void Level10Video() => PlaySequence(9);

    private void PlaySequence(int clipIndex)
    {
        if (disableVideo) return;

        if (gameObject.activeInHierarchy)
        {
            if (clipIndex < levelClips.Length && levelClips[clipIndex] != null)
            {
                if (vPlayer != null) vPlayer.clip = levelClips[clipIndex];
                StartCoroutine(VideoSequenceRoutine());
            }
        }
    }

    private IEnumerator VideoSequenceRoutine()
    {
        if (videoPlayerObject != null)
        {
            videoPlayerObject.SetActive(true);
            if (vPlayer != null) vPlayer.Stop();
        }

        yield return new WaitForSeconds(1.5f);

        if (videoPlayerBackground != null)
        {
            videoPlayerBackground.SetActive(true);
            videoPlayerBackground.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
            yield return StartCoroutine(LerpScale(videoPlayerBackground, new Vector3(20f, 0.1f, 1f), 0.7f));
            yield return StartCoroutine(LerpScale(videoPlayerBackground, new Vector3(20f, 20f, 1f), 0.7f));
        }

        if (vPlayer != null) vPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer source)
    {
        StartCoroutine(DeactivateAfterDelay());
    }

    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(delayAfterEnd);
        if (vPlayer != null) vPlayer.Stop();

        if (videoPlayerBackground != null)
        {
            yield return StartCoroutine(LerpScale(videoPlayerBackground, new Vector3(20f, 0.1f, 1f), 0.7f));
            yield return StartCoroutine(LerpScale(videoPlayerBackground, new Vector3(0.1f, 0.1f, 1f), 0.7f));
            videoPlayerBackground.SetActive(false);
        }

        if (videoPlayerObject != null) videoPlayerObject.SetActive(false);
    }

    private IEnumerator LerpScale(GameObject targetObj, Vector3 targetScale, float duration)
    {
        Vector3 startScale = targetObj.transform.localScale;
        float elapsed = 0;
        while (elapsed < duration)
        {
            targetObj.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        targetObj.transform.localScale = targetScale;
    }

    private void OnDestroy()
    {
        if (vPlayer != null) vPlayer.loopPointReached -= OnVideoFinished;
    }
}