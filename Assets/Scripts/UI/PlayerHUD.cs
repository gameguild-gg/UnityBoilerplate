using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerStats stats;

    [Header("Health")]
    [SerializeField] private RectTransform healthFill;
    [SerializeField] private Image healthFillImage; //used for the hit flash
    [SerializeField] private TMP_Text healthText;        

    [Header("State Meter")]
    [SerializeField] private RectTransform stateFill;
    [SerializeField] private Image stateFillImage; //colored by perception level
    [SerializeField] private TMP_Text stateText;            
    [SerializeField] private RectTransform glimpsedMarker;  
    [SerializeField] private RectTransform visibleMarker;   

    [Header("Perception")]
    [SerializeField] private TMP_Text perceptionText;

    [Header("Colors")]
    [SerializeField] private Color healthColor = new Color(0.85f, 0.15f, 0.2f);
    [SerializeField] private Color hitFlashColor = Color.white;
    [SerializeField] private Color unseenColor = new Color(0.6f, 0.6f, 0.6f);
    [SerializeField] private Color glimpsedColor = new Color(0.75f, 0.55f, 1f);
    [SerializeField] private Color visibleColor = new Color(0.45f, 0.9f, 1f);

    private void OnEnable()
    {
        if (stats == null)
        {
            Debug.LogError("PlayerHUD: Drag the Player's PlayerStats into the 'Stats' slot.", this);
            enabled = false;
            return;
        }

        stats.OnHealthChanged += UpdateHealth;
        stats.OnStateMeterChanged += UpdateStateMeter;
        stats.OnPerceptionChanged += UpdatePerception;
        stats.OnDied += ShowDead;
    }

    private void OnDisable()
    {
        if (stats == null) return;
        stats.OnHealthChanged -= UpdateHealth;
        stats.OnStateMeterChanged -= UpdateStateMeter;
        stats.OnPerceptionChanged -= UpdatePerception;
        stats.OnDied -= ShowDead;
    }

    private void Start()
    {
        if (stats == null) return;

        //it draws the starting values once
        UpdateHealth(stats.CurrentHealth, stats.MaxHealth);
        UpdateStateMeter(stats.StateMeter, stats.MaxStateMeter);
        UpdatePerception(stats.CurrentPerception);

        //it puts the threshold marks in the right spot on the bar
        PlaceMarker(glimpsedMarker, stats.GlimpsedThreshold / stats.MaxStateMeter);
        PlaceMarker(visibleMarker, stats.VisibleThreshold / stats.MaxStateMeter);
    }

    private void Update()
    {
        //it blinks the health bar while the player is invincible after a hit
        if (healthFillImage == null || stats == null) return;
        bool flashOn = stats.IsInvincible && Mathf.FloorToInt(Time.time * 10f) % 2 == 0;
        healthFillImage.color = flashOn ? hitFlashColor : healthColor;
    }

    private void UpdateHealth(float current, float max)
    {
        SetFill(healthFill, max > 0f ? current / max : 0f);
        if (healthText != null)
            healthText.text = $"Health {current:0.#} / {max:0.#}  ({current / 2f:0.#} hearts)";
    }

    private void UpdateStateMeter(float current, float max)
    {
        SetFill(stateFill, max > 0f ? current / max : 0f);
        if (stateText != null)
            stateText.text = $"State Meter {current:0} / {max:0}";
    }

    private void UpdatePerception(Perception perception)
    {
        Color c = perception.perceptionLevel switch
        {
            Perception.LevelName.Visible => visibleColor,
            Perception.LevelName.Glimpsed => glimpsedColor,
            _ => unseenColor
        };

        if (perceptionText != null)
        {
            perceptionText.text = $"Perception: {perception.perceptionLevel}";
            perceptionText.color = c;
        }
        if (stateFillImage != null)
            stateFillImage.color = c;
    }

    private void ShowDead()
    {
        if (perceptionText != null)
        {
            perceptionText.text = "DEAD - restarting...";
            perceptionText.color = healthColor;
        }
    }

    private static void SetFill(RectTransform fill, float value01)
    {
        if (fill == null) return;
        Vector2 max = fill.anchorMax;
        max.x = Mathf.Clamp01(value01);
        fill.anchorMax = max;
    }

    private static void PlaceMarker(RectTransform marker, float value01)
    {
        if (marker == null) return;
        float x = Mathf.Clamp01(value01);
        marker.anchorMin = new Vector2(x, marker.anchorMin.y);
        marker.anchorMax = new Vector2(x, marker.anchorMax.y);
        marker.anchoredPosition = new Vector2(0f, marker.anchoredPosition.y);
    }
}
