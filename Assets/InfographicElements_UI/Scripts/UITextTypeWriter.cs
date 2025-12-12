using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UITextTypeWriter : MonoBehaviour
{
    // The Text component we are typing into
    private Text txt;
    // The full story text (cached at Start)
    private string fullStory;
    // The index of the next character to be typed
    private int characterIndex = 0;
    // The delay between characters
    [SerializeField] private float typeSpeed = 0.125f;
    // Reference to the running coroutine to stop it cleanly
    private Coroutine typeCoroutine;

    void Awake()
    {
        // Get the component early
        txt = GetComponent<Text>();
        // Cache the original text
        fullStory = txt.text;
    }

    void OnEnable()
    {
        // 1. Reset the displayed text to the current progress
        // This handles cases where the object was disabled mid-typing
        txt.text = fullStory.Substring(0, characterIndex);

        // 2. Only start the typing coroutine if the text is NOT finished
        if (characterIndex < fullStory.Length)
        {
            // Start the coroutine and keep a reference to it
            typeCoroutine = StartCoroutine(PlayText());
        }
        // If it is finished, we don't need to do anything, the text is already fullStory
    }

    void OnDisable()
    {
        // If the object is deactivated, stop the current coroutine cleanly
        // to prevent errors and allow it to be restarted later.
        if (typeCoroutine != null)
        {
            StopCoroutine(typeCoroutine);
            typeCoroutine = null; // Clear the reference
        }
    }

    IEnumerator PlayText()
    {
        // Loop from the current characterIndex until the end of the full story
        for (; characterIndex < fullStory.Length; characterIndex++)
        {
            // Append the next character
            txt.text += fullStory[characterIndex];
            yield return new WaitForSeconds(typeSpeed);
        }

        // Optional: The typing is finished, we can clear the coroutine reference
        typeCoroutine = null;
    }
}