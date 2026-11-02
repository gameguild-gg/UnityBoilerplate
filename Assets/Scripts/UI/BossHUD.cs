using System.Data;
using TMPro;
//using UnityEditor.Search;
using UnityEngine;

public class BossHUD : MonoBehaviour
{
    [SerializeField] private ScarcityRemnant remnant;

    [Header("UI Pieces")]
    [Tooltip("the while boss HUD group and it is hidden unitll the fight starts")]
    [SerializeField] private GameObject root;
    [SerializeField] private RectTransform scarcityFill;
    [SerializeField] private UnityEngine.UI.Image scarcityFillImage; // colored by phase
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text scarcityText;
    [SerializeField] private TMP_Text phaseText;
    [SerializeField] private TMP_Text stateText;

    [Header("Text")]
    [SerializeField] private string bossName = "The first Trial the Scarcity Remnant";
    [SerializeField] private string winMessage = "Scarcity transformed ito abundance. ";

    [Header("Phase Colors")]
    [SerializeField] private Color weakenedColor = new Color(0.75f, 0, 65f, 0.45f);
    [SerializeField] private Color stableColor = new Color(0.8f, 0.45f, 0.15f);
    [SerializeField] private Color ravenousColor = new Color(0.6f, 0.08f, 0.08f);
    [SerializeField] private Color transformedColor = new Color(1f, 0.85f, 0.4f);

    private void OnEnable()
    {
        if(remnant == null)
        {
            Debug.LogError("BossHud: Drag the boss's scarcity remnant into the premant slot ");
            enabled = false;
            return;
        }
        remnant.OnEncounterStarted += Show;
        remnant.OnScarcityChanged += UpdateScarcity;
        remnant.OnPhaseChanged += UpdatePhase;
        remnant.OnStateChanged += UpdateState;
        remnant.OnTransformed += ShowWin;
    }

    private void OnDisable()
    {
        if (remnant == null) return;

        remnant.OnEncounterStarted -= Show;
        remnant.OnScarcityChanged -= UpdateScarcity;
        remnant.OnPhaseChanged -= UpdatePhase;
        remnant.OnStateChanged -= UpdateState;
        remnant.OnTransformed -= ShowWin;
    }

    private void Start()
    {
        if (remnant == null) return;

        if (nameText != null) nameText.text = bossName;

        UpdateScarcity(remnant.Scarcity, remnant.MaxScarcity);
        UpdatePhase(remnant.Phase);
        UpdateState(remnant.State);
        if (root != null) root.SetActive(remnant.State != RemnantState.Dormant);
    }

    private void Show()
    {
        if (root != null) root.SetActive(true);
    }

    private void UpdateScarcity(float current, float max)
    {
        if(scarcityFill != null)
        {
            Vector2 a = scarcityFill.anchorMax;
            a.x = max > 0f ? Mathf.Clamp01(current / max) : 0f;
            scarcityFill.anchorMax = a;
        }
        if (scarcityText != null)
        {
            scarcityText.text = $"Scarcity {current:0} / {max:0}";
        }
    }

    private void UpdatePhase(RemnantPhase phase)
    {
        Color c = stableColor;
        if (phase == RemnantPhase.Weakened) c = weakenedColor;
        else if (phase == RemnantPhase.Ravenous) c = ravenousColor;
        if (scarcityFillImage != null) scarcityFillImage.color = c;
        if(phaseText != null)
        {
            phaseText.text = phase.ToString().ToUpper();
            phaseText.color = c;
        }
    }

    private void UpdateState(RemnantState state)
    {
        if (stateText != null) stateText.text = $"State: {state}";
    }

    private void ShowWin()
    {
        if (scarcityFillImage != null) scarcityFillImage.color = transformedColor;
        if (phaseText != null)
        {
            phaseText.text = "TRANSFORMED";
            phaseText.color = transformedColor;
        }
        if (scarcityText != null) scarcityText.text = winMessage;
    }
}
