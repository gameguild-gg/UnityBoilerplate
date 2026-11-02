using UnityEngine;

public class EncounterEnvironment : MonoBehaviour
{
    [SerializeField] private ScarcityRemnant remnant;

    [Header("Darkness overlay (a big square sprite covering the room)")]
    [SerializeField] private SpriteRenderer darknessOverlay;
    [SerializeField] private Color overlayColor = new Color(0.12f, 0.02f, 0.05f);
    [Tooltip("Overlay alpha when Scarcity is 0.")]
    [SerializeField, Range(0f, 1f)] private float minDarkness = 0.05f;
    [Tooltip("Overlay alpha when Scarcity is max.")]
    [SerializeField, Range(0f, 1f)] private float maxDarkness = 0.65f;

    [Header("Camera background (optional)")]
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Color calmBackground = new Color(0.12f, 0.12f, 0.16f);
    [SerializeField] private Color scarcityBackground = new Color(0.08f, 0.01f, 0.02f);
    [SerializeField] private Color transformedBackground = new Color(0.25f, 0.2f, 0.12f);

    [Header("Feel")]
    [SerializeField] private float fadeSpeed = 2f;
    [Tooltip("Before the fight starts, show this much of the scarcity darkness (0 = none).")]
    [SerializeField, Range(0f, 1f)] private float dormantStrength = 0.3f;

    private float target01;
    private float current01;
    private bool transformed;
    private bool started;


    private void OnDisable()
    {
        if (remnant == null) return;
        remnant.OnScarcityChanged -= HandleScarcity;
        remnant.OnEncounterStarted -= HandleStarted;
        remnant.OnTransformed -= HandleTransformed;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (remnant == null) return;

        target01 = remnant.Scarcity01 * dormantStrength;
        current01 = target01;
        Apply();
    }

    private void HandleStarted()
    {
        started = true;
        target01 = remnant.Scarcity01;
    }

    private void HandleScarcity(float current, float max)
    {
        if (transformed) return;

        float s01 = max > 0f ? current / max : 0f;
        target01 = started ? s01 : s01 * dormantStrength;
    }

    private void HandleTransformed()
    {
        transformed = true;
        target01 = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        current01 = Mathf.MoveTowards(current01, target01, fadeSpeed * Time.deltaTime);
        Apply();
    }

    private void Apply()
    {
        if (darknessOverlay != null)
        {
            float a = transformed ? Mathf.Lerp(0f, minDarkness, current01) : Mathf.Lerp(minDarkness, maxDarkness, current01);
            darknessOverlay.color = new Color(overlayColor.r, overlayColor.g, overlayColor.b, a);
        }

        if (targetCamera != null)
        {
            targetCamera.backgroundColor = transformed ? Color.Lerp(transformedBackground, calmBackground, current01) : Color.Lerp(calmBackground, scarcityBackground, current01);
        }
    }
}
