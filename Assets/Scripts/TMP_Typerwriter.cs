using System.Collections;
using UnityEngine;
using TMPro;

public class TMP_Typewriter : MonoBehaviour
{
    private TextMeshProUGUI txt;
    private string fullStory;
    private int characterIndex = 0;

    [Header("Settings")]
    [SerializeField] private float typeSpeed = 0.05f;

    private Coroutine typeCoroutine;

    void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();

        if (txt != null)
        {
            // Cache the full text from the inspector once
            fullStory = txt.text;
            // Blank it out so it doesn't flash the full text on load
            txt.text = "";
        }
    }

    void OnEnable()
    {
        // Stop any existing routine to avoid double-typing
        if (typeCoroutine != null)
        {
            StopCoroutine(typeCoroutine);
        }

        // RESET: Start from character 0 every time this object is activated
        characterIndex = 0;

        if (txt != null && !string.IsNullOrEmpty(fullStory))
        {
            txt.text = "";
            typeCoroutine = StartCoroutine(PlayText());
        }
    }

    void OnDisable()
    {
        // Clean up when the object is hidden
        if (typeCoroutine != null)
        {
            StopCoroutine(typeCoroutine);
            typeCoroutine = null;
        }
    }

    IEnumerator PlayText()
    {
        // Optional: slight delay to let UI transitions finish
        yield return new WaitForSecondsRealtime(0.05f);

        for (; characterIndex < fullStory.Length; characterIndex++)
        {
            txt.text += fullStory[characterIndex];

            // Realtime ignores Time.timeScale = 0 (Pause/Win/Fail states)
            yield return new WaitForSecondsRealtime(typeSpeed);
        }

        typeCoroutine = null;
    }
}