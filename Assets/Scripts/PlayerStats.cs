using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 6f; //hearts
    [SerializeField] private float currentHealth = 6f;
    [Tooltip("Seconds of invincibility after taking a hit, so you can't be hit 10 times in one frame.")]
    [SerializeField] private float invincibilityTime = 1f;

    [Header("State Meter")]
    [SerializeField] private float maxStateMeter = 100f;
    [SerializeField] private float stateMeter = 50f;

    [Tooltip("At or above this, hidden entities start to be glimpsed.")]
    [SerializeField] private float glimpsedThreshold = 40f;
    [Tooltip("At or above this, hidden entities are fully visible.")]
    [SerializeField] private float visibleThreshold = 75f;

    [Header("Other (not used yet)")]
    [SerializeField] private float damageTakenMultiplier = 1f; //raise this when aggression lowers the State Meter
    [SerializeField] private int helpfulActionsDone; //civilians helped, things repaired, etc.
    [SerializeField] private int aggressiveActionsDone; //attacks on the signature enemy, etc.

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDead => currentHealth <= 0f;
    public bool IsInvincible => invincibleTimer > 0f;

    public float StateMeter => stateMeter;
    public float MaxStateMeter => maxStateMeter;
    public float StateMeter01 => maxStateMeter > 0f ? stateMeter / maxStateMeter : 0f;
    public float GlimpsedThreshold => glimpsedThreshold;
    public float VisibleThreshold => visibleThreshold;

    public float DamageTakenMultiplier
    {
        get => damageTakenMultiplier;
        set => damageTakenMultiplier = Mathf.Max(0f, value);
    }

    public int HelpfulActionsDone => helpfulActionsDone;
    public int AggressiveActionsDone => aggressiveActionsDone;

    //Unseen / Glimpsed / Visible, based on the State Meter thresholds above
    public Perception CurrentPerception
    {
        get
        {
            Perception perception;
            perception.levelValue = stateMeter / maxStateMeter;

            if (stateMeter >= visibleThreshold) {
                perception.perceptionLevel = Perception.LevelName.Visible;
            }
            if (stateMeter >= glimpsedThreshold) {
                perception.perceptionLevel = Perception.LevelName.Glimpsed;
            }
            else {
                perception.perceptionLevel = Perception.LevelName.Unseen;
            }
            return perception;
        }
    }

    public event Action<float, float> OnHealthChanged; //max
    public event Action<float> OnDamaged; //amount taken
    public event Action<float> OnHealed; //amount healed
    public event Action OnDied;
    public event Action<float, float> OnStateMeterChanged; //max
    public event Action<Perception> OnPerceptionChanged;

    private float invincibleTimer;
    private Perception lastPerception;

    private void Awake()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        stateMeter = Mathf.Clamp(stateMeter, 0f, maxStateMeter);
        lastPerception = CurrentPerception;
    }

    private void Start()
    {
        //it lets any UI draw the starting values.
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnStateMeterChanged?.Invoke(stateMeter, maxStateMeter);
    }

    private void Update()
    {
        if (invincibleTimer > 0f)
            invincibleTimer -= Time.deltaTime;
    }

    //health
    public void TakeDamage(float amount)
    {
        if (IsDead || IsInvincible || amount <= 0f) return;

        float finalAmount = amount * damageTakenMultiplier;
        currentHealth = Mathf.Max(0f, currentHealth - finalAmount);
        invincibleTimer = invincibilityTime;

        OnDamaged?.Invoke(finalAmount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (IsDead)
            OnDied?.Invoke();
    }

    //Instantly kills the player, ignoring invincibility
    public void Kill()
    {
        if (IsDead) return;
        currentHealth = 0f;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnDied?.Invoke();
    }

    public void Heal(float amount)
    {
        if (IsDead || amount <= 0f) return;

        float before = currentHealth;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);

        OnHealed?.Invoke(currentHealth - before);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void SetMaxHealth(float newMax, bool fillToMax = false)
    {
        maxHealth = Mathf.Max(1f, newMax);
        currentHealth = fillToMax ? maxHealth : Mathf.Min(currentHealth, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    //it is for Positive = improve (helpful actions). Negative = worsen (aggression)
    public void ChangeStateMeter(float amount)
    {
        SetStateMeter(stateMeter + amount);
    }

    public void SetStateMeter(float value)
    {
        stateMeter = Mathf.Clamp(value, 0f, maxStateMeter);
        OnStateMeterChanged?.Invoke(stateMeter, maxStateMeter);

        Perception now = CurrentPerception;
        if (now.levelValue != lastPerception.levelValue)
        {
            lastPerception = now;
            OnPerceptionChanged?.Invoke(now);
        }
    }

    public void RegisterHelpfulAction() => helpfulActionsDone++;
    public void RegisterAggressiveAction() => aggressiveActionsDone++;

#if UNITY_EDITOR
    //it keeps values sane when you edit them in the Inspector
    private void OnValidate()
    {
        maxHealth = Mathf.Max(1f, maxHealth);
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        maxStateMeter = Mathf.Max(1f, maxStateMeter);
        stateMeter = Mathf.Clamp(stateMeter, 0f, maxStateMeter);
        visibleThreshold = Mathf.Max(glimpsedThreshold, visibleThreshold);
    }
#endif
}

//it is how well the player can perceive hidden entities
//public enum PerceptionLevel
//{
//    Unseen,
//    Glimpsed,
//    Visible
//}

public struct Perception
{
    public enum LevelName
    {
        Unseen,
        Glimpsed,
        Visible
    }
    public LevelName perceptionLevel;
    public float levelValue; // 0-1 
}
