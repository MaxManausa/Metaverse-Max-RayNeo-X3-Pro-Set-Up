using UnityEngine;
using System.Collections;

public class HomeMenuSequencer : MonoBehaviour
{
    [Header("Sequence Objects")]
    [SerializeField] private GameObject secretMessage;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject earthSatellite;
    [SerializeField] private GameObject startText;

    [Header("Instructions Settings")]
    [SerializeField] private CanvasGroup instructionsGroup;
    [SerializeField] private RectTransform instructionsRect; // Drag the same object here

    private void OnEnable()
    {
        ResetAll();
        StartCoroutine(PlayHomeSequence());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        ResetAll();
    }

    private void ResetAll()
    {
        if (secretMessage) secretMessage.SetActive(false);

        // Reset Title Scale
        if (title)
        {
            title.transform.localScale = Vector3.one * 0.1f;
            title.SetActive(false);
        }

        // Reset Earth Scale
        if (earthSatellite)
        {
            earthSatellite.transform.localScale = Vector3.one * 0.1f;
            earthSatellite.SetActive(false);
        }

        if (startText) startText.SetActive(false);

        // Reset Instructions Scale and Alpha
        if (instructionsGroup)
        {
            instructionsGroup.alpha = 0;
            instructionsRect.localScale = Vector3.one * 0.1f;
            instructionsGroup.gameObject.SetActive(false);
        }
    }

    IEnumerator PlayHomeSequence()
    {
        // 1. Secret Message (Static)
        secretMessage.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        secretMessage.SetActive(false);

        // 2. Title Scale (1 second)
        title.SetActive(true);
        yield return StartCoroutine(ScaleOverTime(title.transform, 1.0f));

        // 3. Earth & Satellite Scale (3 seconds)
        earthSatellite.SetActive(true);
        yield return StartCoroutine(ScaleOverTime(earthSatellite.transform, 3.0f));

        // 4. Start Text appears immediately
        startText.SetActive(true);

        // 5. Instructions (Wait 3s, then Fade AND Scale over 3s)
        yield return new WaitForSeconds(3.0f);
        instructionsGroup.gameObject.SetActive(true);
        yield return StartCoroutine(FadeAndScaleInstructions(3.0f));
    }

    // Helper for simple scaling
    IEnumerator ScaleOverTime(Transform target, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;
            target.localScale = Vector3.Lerp(Vector3.one * 0.1f, Vector3.one, percent);
            yield return null;
        }
        target.localScale = Vector3.one;
    }

    // Helper for simultaneous Fade and Scale
    IEnumerator FadeAndScaleInstructions(float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;

            instructionsGroup.alpha = percent;
            instructionsRect.localScale = Vector3.Lerp(Vector3.one * 0.1f, Vector3.one, percent);

            yield return null;
        }
        instructionsGroup.alpha = 1;
        instructionsRect.localScale = Vector3.one;
    }
}