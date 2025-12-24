using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class SEDVideoPlayer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject videoPlayerObject;      // The Parent (contains VideoPlayer)
    [SerializeField] private GameObject videoPlayerBackground;  // The Child (the scaling frame)

    [Header("Level Video Clips")]
    [Tooltip("Assign 10 videos here. Element 0 = Level 1, Element 1 = Level 2, etc.")]
    [SerializeField] private VideoClip[] levelClips = new VideoClip[10];

    private VideoPlayer vPlayer;

    [Header("Settings")]
    public float delayAfterEnd = 1.0f;

    private void Awake()
    {
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

    // --- Public Methods called by ScoreManager ---
    // These methods trigger the sequence with the specific clip for that level

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
        if (gameObject.activeInHierarchy)
        {
            // Ensure the clip exists before starting
            if (clipIndex < levelClips.Length && levelClips[clipIndex] != null)
            {
                vPlayer.clip = levelClips[clipIndex];
                StartCoroutine(VideoSequenceRoutine());
            }
            else
            {
                Debug.LogWarning($"Video Clip for Level {clipIndex + 1} is missing!");
            }
        }
    }

    private IEnumerator VideoSequenceRoutine()
    {
        // 1. ACTIVATE THE PARENT
        if (videoPlayerObject != null)
        {
            videoPlayerObject.SetActive(true);
            if (vPlayer != null) vPlayer.Stop();
        }

        // 2. Wait 1.5s
        yield return new WaitForSeconds(1.5f);

        if (videoPlayerBackground != null)
        {
            // 3. Activate the Child Background and set to point scale
            videoPlayerBackground.SetActive(true);
            videoPlayerBackground.transform.localScale = new Vector3(0.1f, 0.1f, 1f);

            // 4. Horizontal Stretch (X)
            yield return StartCoroutine(LerpScale(videoPlayerBackground, new Vector3(20f, 0.1f, 1f), 0.7f));

            // 5. Vertical Stretch (Y)
            yield return StartCoroutine(LerpScale(videoPlayerBackground, new Vector3(20f, 20f, 1f), 0.7f));
        }

        // 6. Play the video (the clip was set in PlaySequence)
        if (vPlayer != null) vPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer source)
    {
        StartCoroutine(DeactivateAfterDelay());
    }

    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(delayAfterEnd);

        // 7. Stop the video
        if (vPlayer != null) vPlayer.Stop();

        if (videoPlayerBackground != null)
        {
            // 8. Reverse: Shrink Height (Y)
            yield return StartCoroutine(LerpScale(videoPlayerBackground, new Vector3(20f, 0.1f, 1f), 0.7f));

            // 9. Reverse: Shrink Width (X)
            yield return StartCoroutine(LerpScale(videoPlayerBackground, new Vector3(0.1f, 0.1f, 1f), 0.7f));

            videoPlayerBackground.SetActive(false);
        }

        // 10. Deactivate Parent
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