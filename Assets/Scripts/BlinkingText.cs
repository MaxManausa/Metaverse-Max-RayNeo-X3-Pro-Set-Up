using UnityEngine;
using TMPro;

public class BlinkingText : MonoBehaviour
{
    private TextMeshProUGUI _textComponent;

    [Header("Pulse Settings")]
    public float pulseSpeed = 1.5f;
    public float minAlpha = 0.1f;
    public float maxAlpha = 1.0f;

    void Awake()
    {
        _textComponent = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (_textComponent == null) return;

        // Using unscaledTime ensures the menu still "breathes" even when the game is paused
        float t = Mathf.PingPong(Time.unscaledTime * pulseSpeed, 1);

        // Calculate the new alpha (transparency)
        float newAlpha = Mathf.Lerp(minAlpha, maxAlpha, t);

        // Apply it to the TextMeshPro color property
        Color c = _textComponent.color;
        c.a = newAlpha;
        _textComponent.color = c;
    }

    // When the screen is hidden, ensure it resets to full visibility for next time
    void OnDisable()
    {
        if (_textComponent != null)
        {
            Color c = _textComponent.color;
            c.a = maxAlpha;
            _textComponent.color = c;
        }
    }
}