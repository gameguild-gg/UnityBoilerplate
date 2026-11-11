using UnityEngine;
using System;

public enum RemnantState
{
    Dormant, //waiting for the encounter to start
    Awakening, //intro: it riese up and can not be attacked yet
    Stalking, //slowly follows the player and picks an attack
    TelegraphSlam, //warning circle on the ground
    Slam, //area hit around the boss
    TelegraphLunge, //warning lane toward the player
    Lunge, //fast charge along the lane
    Recover, //short pause after an attack
    Feeding, //reacting to being attacked and grows, roars
    Weakening, //reacting to a helpful action, stagger back and shrinks
    Transforming, //scarcity hits 0 and turining into abundance
    Transformed //the encouter is won

}

public enum RemnantPhase
{
    Weakened, //low scarcity and small slow slams only
    Stable, //meduim slams and lunges
    Ravenous //high scarcity big fast, double slams, hits hard
}

[RequireComponent(typeof(Rigidbody2D))]
public class ScarcityRemnant : MonoBehaviour
{
    [Header("Refereances")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Transform player;
    [Tooltip("child obj hols the boss sprite and coloder and is it what should grow and shrink")]
    [SerializeField] private Transform body;
    [SerializeField] private SpriteRenderer bodyRenderer;
    [Tooltip("circle sprite the child of the boss onot the body and it is used as the slam warning")]
    [SerializeField] private SpriteRenderer slamTelegraph;
    [Tooltip("square sprite child of the boss and not the body and is used fo the lunge warning lane")]
    [SerializeField] private SpriteRenderer lungeTelegraph;

    [Header("Encounter")]
    [Tooltip("start the fight as soon as play is prosed or just trigger with debug 7")]
    [SerializeField] private bool startOnPlay = false;
    [SerializeField] private float awakenTime = 1.5f;

    [Header("Scarcity and it goes up when attacked and down when helped")]
    [SerializeField] private float maxScarcity = 100f;
    [SerializeField] private float startScarcity = 50f;
    [SerializeField] private float scarcityPerAggression = 12f;
    [SerializeField] private float scarcityPerHelp = 15f;

    [Header("Effct on the player")]
    [SerializeField] private float stateMeterLossPerAggression = 8f;
    [SerializeField] private float stateMeterGainPerHelp = 10f;
    [Tooltip("Damage player and it takes is multiped by this and goes from min boss starved to max boss gorged")]
    [SerializeField] private float playerDamageMultiplierMin = 0.75f;
    [SerializeField] private float playerDamageMultiplierMax = 1.75f;
    [SerializeField] private float stateMeterBonusOnTransform = 25f;

    [Header("Phases by scarcity")]
    [SerializeField] private float weakenedBelow = 30f;
    [SerializeField] private float ravenousAbove = 70f;

    [Header("Size")]
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 2.2f;
    [SerializeField] private float scaleChangeSpeed = 4f;

    [Header("Movement")]
    [SerializeField] private float stalkSpeedMin = 1.0f;
    [SerializeField] private float stalkSpeedMax = 2.4f;

    [Header("Attack Timing")]
    [SerializeField] private float cooldownWhenWeak = 2.4f;
    [SerializeField] private float cooldownWhenRavenous = 1.2f;
    [SerializeField] private float recoverTime = 0.6f;
    [Tooltip("Extra wait after the boss lands a hit on the player")]
    [SerializeField] private float breatherAfterHit = 1.0f;
    [Tooltip("at or bellow this palyer health cool downs are multiped by a mercy cooldown mutiplyer")]
    [SerializeField] private float mercyHealthThreshold = 2f;
    [SerializeField] private float mercyCooldownMultiplier = 1.5f;
    [Tooltip("no telegraph is ever shorter than this for the fairness")]
    [SerializeField] private float minTelegraphTime = 0.6f;

    [Header("Slam area attack around the boss")]
    [Tooltip("radius at scale 1 and it grows with da boss")]
    [SerializeField] private float slamRadius = 1.4f;
    [SerializeField] private float slamTelegraphTime = 0.9f;
    [SerializeField] private float slamTelegraphTimeRavenous = 0.7f;
    [SerializeField] private float slamDamageMin = 1f;
    [SerializeField] private float slamDamageMax = 2f;
    [SerializeField] private bool ravenousDoubleSlam = true;

    [Header("Lunge charge towrs the player")]
    [Tooltip("only lunges if the palyer is at lease this far away")]
    [SerializeField] private float lungeMinDistance = 3.5f;
    [SerializeField] private float lungeTelegraphTime = 0.8f;
    [SerializeField] private float lungeSpeed = 9f;
    [SerializeField] private float lungeDuration = 0.45f;
    [SerializeField] private float lungeDamageMin = 1f;
    [SerializeField] private float lungeDamageMax = 2f;
    [SerializeField] private float lungeLaneWidth = 0.8f;

    [Header("Touching the boss")]
    [SerializeField] private float contactDamage = 1f;
    [Tooltip("Body raduis ar scale 1 and is used for contact adn lunge hits")]
    [SerializeField] private float bodyRadius = 0.5f;
    [Tooltip("rough radius of the palyer and added ot hit checks ")]
    [SerializeField] private float playerRadius = 0.3f;

    [Header("Reactions")]
    [SerializeField] private float feedingTime = 0.5f;
    [SerializeField] private float weakenStaggerTime = 0.8f;
    [SerializeField] private float weakenKnockbackSpeed = 3f;
    [SerializeField] private float transformTime = 2.5f;

    [Header("Colors")]
    [Tooltip("tints the whole sprite by phase and turn this off when using the golem art so its own colors show (flashes still work)")]
    [SerializeField] private bool tintBodyByPhase = true;
    [SerializeField] private Color dormantColor = new Color(0.35f, 0.35f, 0.35f);
    [SerializeField] private Color weakenedColor = new Color(0.75f, 0.65f, 0.45f);
    [SerializeField] private Color stableColor = new Color(0.8f, 0.45f, 0.15f);
    [SerializeField] private Color ravenousColor = new Color(0.6f, 0.08f, 0.08f);
    [SerializeField] private Color fedFlashColor = new Color(1f, 0.25f, 0.1f);
    [SerializeField] private Color helpedFlashColor = new Color(0.6f, 1f, 0.7f);
    [SerializeField] private Color transformedColor = new Color(1f, 0.85f, 0.4f);
    [SerializeField] private Color telegraphColor = new Color(1f, 0.2f, 0.1f, 0.6f);



    public RemnantState State { get; private set; } = RemnantState.Dormant;
    public RemnantPhase Phase { get; private set; } = RemnantPhase.Stable;
    public float Scarcity { get; private set; }
    public float MaxScarcity => maxScarcity;
    public float Scarcity01 => maxScarcity > 0f ? Scarcity / maxScarcity : 0f;
    public float CurrentScale => currentScale;
    //for the BossSpriteAnimator: 0 at the start of the current state and 1 at the end (used to time the telegraph animations)
    public float StateProgress01 => stateDuration > 0f ? Mathf.Clamp01(1f - stateTimer / stateDuration) : 1f;
    public Transform Player => player;
    public Vector2 LungeDirection => lungeDirection;
    public bool IsActive => State != RemnantState.Dormant && State != RemnantState.Transforming && State != RemnantState.Transformed;
    public event Action OnEncounterStarted;
    public event Action<float, float> OnScarcityChanged;
    public event Action<RemnantPhase> OnPhaseChanged;
    public event Action<RemnantState> OnStateChanged;
    public event Action OnFed; //attacked
    public event Action OnWeakened; //helped
    public event Action OnHitPlayer;
    public event Action OnTransformed; //encounter won



    private Rigidbody2D rb;
    private float stateTimer;
    private float stateDuration;
    private float attackCooldownTimer;
    private bool hitPlayerThisAttack;
    private bool doubleSlamDone;
    private bool lungeHasHit;
    private Vector2 lungeDirection;
    private bool playerDead;
    private float currentScale;
    private float punch;
    private Color flashColor;
    private float flashTimer;
    private const float FlashDuration = 0.35f;






    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.mass = 20f;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        if (playerStats == null || player == null || body == null)
        {
            Debug.LogError("Scarcity remant drag in playe rsatus and player and body in the inspercor");
        }

        Scarcity = Mathf.Clamp(startScarcity, 0f, maxScarcity);
        Phase = CalculatePhase();
        currentScale = minScale * 0.5f;
        HideTelegraphs();
    }

    private void OnEnable()
    {
        if (playerStats != null) playerStats.OnDied += HandlePlayerDied;
    }

    private void OnDisable()
    {
        if (playerStats != null) playerStats.OnDied -= HandlePlayerDied;
    }

    private void Start()
    {
        OnScarcityChanged?.Invoke(Scarcity, maxScarcity);
        OnPhaseChanged?.Invoke(Phase);
        OnStateChanged?.Invoke(State);
        ApplyBodyScale(true);
        if (startOnPlay) StartEncounter();
    }

    private void Update()
    {
        UpdateVisuals();
        if (State == RemnantState.Dormant || State == RemnantState.Transformed) return;

        if (playerStats == null || player == null || body == null) return;

        float dt = Time.deltaTime;
        stateTimer -= dt;
        attackCooldownTimer -= dt;

        switch (State)
        {
            case RemnantState.Awakening:
                if (stateTimer <= 0f) EnterState(RemnantState.Stalking);
                break;

            case RemnantState.Stalking:
                CheckContactDamage();
                if (!playerDead) TryChooseAttack();
                break;

            case RemnantState.TelegraphSlam:
                UpdateSlamTelegraph();
                if (stateTimer <= 0f) EnterState(RemnantState.Slam);
                break;

            case RemnantState.Slam:
                if (stateTimer <= 0f)
                {
                    bool canDouble = ravenousDoubleSlam && Phase == RemnantPhase.Ravenous
                                     && !doubleSlamDone && !playerDead;
                    if (canDouble)
                    {
                        doubleSlamDone = true;
                        EnterState(RemnantState.TelegraphSlam);
                    }
                    else EnterState(RemnantState.Recover);
                }
                break;

            case RemnantState.TelegraphLunge:
                UpdateLungeTelegraph();
                if (stateTimer <= 0f) EnterState(RemnantState.Lunge);
                break;

            case RemnantState.Lunge:
                if (stateTimer <= 0f) EnterState(RemnantState.Recover);
                break;

            case RemnantState.Recover:
            case RemnantState.Feeding:
                CheckContactDamage();
                if (stateTimer <= 0f) EnterState(RemnantState.Stalking);
                break;

            case RemnantState.Weakening:
                if (stateTimer <= 0f) EnterState(RemnantState.Stalking);
                break;

            case RemnantState.Transforming:
                if (stateTimer <= 0f) FinishTransformation();
                break;
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = Vector2.zero;

        if (player != null && !playerDead)
        {
            Vector2 toPlayer = (Vector2)player.position - rb.position;
            float dist = toPlayer.magnitude;
            Vector2 dir = dist > 0.001f ? toPlayer / dist : Vector2.zero;

            switch (State)
            {
                case RemnantState.Stalking:
                    //walk toward the player but stop once they're well inside slam range.
                    if (dist > SlamRadius * 0.6f) velocity = dir * StalkSpeed;
                    break;

                case RemnantState.Lunge:
                    velocity = lungeDirection * lungeSpeed;
                    if (!lungeHasHit && dist <= BodyRadius + playerRadius)
                    {
                        lungeHasHit = true;
                        DamagePlayer(Mathf.Round(Mathf.Lerp(lungeDamageMin, lungeDamageMax, Scarcity01)));
                    }
                    break;

                case RemnantState.Weakening:
                    //stagger away from the player, slowing down.
                    float t = stateDuration > 0f ? stateTimer / stateDuration : 0f;
                    velocity = -dir * weakenKnockbackSpeed * Mathf.Clamp01(t);
                    break;
            }
        }

        SetVelocity(velocity);
    }








    //wake the boss up and begin the fight
    public void StartEncounter()
    {
        if (State != RemnantState.Dormant) return;
        Debug.Log("[Remnant] The Scarcity Remnant awakens.");
        UpdatePlayerVulnerability();
        OnEncounterStarted?.Invoke();
        EnterState(RemnantState.Awakening);
    }

    //the player attacked the boss and this feeds the boss
    public void ReceiveAggression(float strength = 1f)
    {
        if (!IsActive || playerStats == null) return;

        ChangeScarcity(scarcityPerAggression * strength);
        playerStats.ChangeStateMeter(-stateMeterLossPerAggression * strength);
        playerStats.RegisterAggressiveAction();

        Flash(fedFlashColor);
        punch = 0.25f;
        OnFed?.Invoke();
        Debug.Log($"[Remnant] Fed by aggression. Scarcity {Scarcity:0}/{maxScarcity:0}");

        //attacking does not interrupt attacks it only roars if it was idleing.
        if (State == RemnantState.Stalking || State == RemnantState.Recover)
            EnterState(RemnantState.Feeding);
    }

    //the player did something helpful this make the boss starve
    public void ReceiveHelp(float strength = 1f)
    {
        if (!IsActive || playerStats == null) return;

        ChangeScarcity(-scarcityPerHelp * strength);
        playerStats.ChangeStateMeter(stateMeterGainPerHelp * strength);
        playerStats.RegisterHelpfulAction();

        Flash(helpedFlashColor);
        punch = -0.2f;
        OnWeakened?.Invoke();
        Debug.Log($"[Remnant] Weakened by help. Scarcity {Scarcity:0}/{maxScarcity:0}");

        if (Scarcity <= 0f)
        {
            EnterState(RemnantState.Transforming);
            return;
        }

        //helping all of the time interrupts, even mid-telegraph and this will be the reward for helping
        EnterState(RemnantState.Weakening);
    }

    //dDebug: skip straight to the transformation
    public void ForceTransform()
    {
        if (!IsActive) return;
        ChangeScarcity(-Scarcity);
        EnterState(RemnantState.Transforming);
    }

    private void EnterState(RemnantState next)
    {
        State = next;
        HideTelegraphs();

        switch (next)
        {
            case RemnantState.Awakening:
                SetTimer(awakenTime);
                break;

            case RemnantState.Stalking:
                SetTimer(0f);
                break;

            case RemnantState.TelegraphSlam:
                float slamWarn = Phase == RemnantPhase.Ravenous ? slamTelegraphTimeRavenous : slamTelegraphTime;
                SetTimer(Mathf.Max(minTelegraphTime, slamWarn));
                if (slamTelegraph != null) slamTelegraph.enabled = true;
                UpdateSlamTelegraph();
                break;

            case RemnantState.Slam:
                SetTimer(0.2f);
                DoSlam();
                break;

            case RemnantState.TelegraphLunge:
                SetTimer(Mathf.Max(minTelegraphTime, lungeTelegraphTime));
                Vector2 toPlayer = (Vector2)player.position - rb.position;
                lungeDirection = toPlayer.sqrMagnitude > 0.001f ? toPlayer.normalized : Vector2.down;
                if (lungeTelegraph != null) lungeTelegraph.enabled = true;
                UpdateLungeTelegraph();
                break;

            case RemnantState.Lunge:
                SetTimer(lungeDuration);
                lungeHasHit = false;
                punch = 0.1f;
                break;

            case RemnantState.Recover:
                SetTimer(recoverTime);
                StartCooldown();
                break;

            case RemnantState.Feeding:
                SetTimer(feedingTime);
                break;

            case RemnantState.Weakening:
                SetTimer(weakenStaggerTime);
                StartCooldown();
                break;

            case RemnantState.Transforming:
                SetTimer(transformTime);
                Debug.Log("[Remnant] Scarcity is gone. It begins to transform...");
                break;
        }

        OnStateChanged?.Invoke(State);
    }

    private void TryChooseAttack()
    {
        if (attackCooldownTimer > 0f) return;

        float dist = Vector2.Distance(player.position, rb.position);

        if (dist <= SlamRadius * 0.9f)
        {
            hitPlayerThisAttack = false;
            doubleSlamDone = false;
            EnterState(RemnantState.TelegraphSlam);
        }
        else if (Phase != RemnantPhase.Weakened && dist >= lungeMinDistance)
        {
            hitPlayerThisAttack = false;
            EnterState(RemnantState.TelegraphLunge);
        }
        //otherwise keep stalking closer
    }

    private void DoSlam()
    {
        punch = 0.3f;
        if (slamTelegraph != null)
        {
            //show the full circle solid for a moment so the hit area is clear.
            slamTelegraph.enabled = true;
            SetTelegraphSize(slamTelegraph, SlamRadius * 2f, SlamRadius * 2f);
            slamTelegraph.color = new Color(telegraphColor.r, telegraphColor.g, telegraphColor.b, 0.9f);
        }

        float dist = Vector2.Distance(player.position, rb.position);
        if (dist <= SlamRadius + playerRadius)
            DamagePlayer(Mathf.Round(Mathf.Lerp(slamDamageMin, slamDamageMax, Scarcity01)));
    }

    private void FinishTransformation()
    {
        State = RemnantState.Transformed;
        SetVelocity(Vector2.zero);
        HideTelegraphs();

        playerStats.DamageTakenMultiplier = 1f;
        playerStats.ChangeStateMeter(stateMeterBonusOnTransform);

        Debug.Log("[Remnant] Transformed. Scarcity has become abundance. Encounter complete.");
        OnStateChanged?.Invoke(State);
        OnTransformed?.Invoke();
    }


    private float SlamRadius => slamRadius * currentScale;
    private float BodyRadius => bodyRadius * currentScale;
    private float StalkSpeed => Mathf.Lerp(stalkSpeedMin, stalkSpeedMax, Scarcity01);

    private float CurrentCooldown
    {
        get
        {
            float cd = Mathf.Lerp(cooldownWhenWeak, cooldownWhenRavenous, Scarcity01);
            if (hitPlayerThisAttack) cd += breatherAfterHit;
            if (playerStats != null && playerStats.CurrentHealth <= mercyHealthThreshold)
                cd *= mercyCooldownMultiplier;
            return cd;
        }
    }

    private void StartCooldown() => attackCooldownTimer = CurrentCooldown;

    private void SetTimer(float seconds)
    {
        stateTimer = seconds;
        stateDuration = seconds;
    }

    private void DamagePlayer(float amount)
    {
        if (playerStats == null || playerStats.IsDead || playerStats.IsInvincible) return;
        playerStats.TakeDamage(amount);
        hitPlayerThisAttack = true;
        OnHitPlayer?.Invoke();
    }

    private void CheckContactDamage()
    {
        if (playerDead || contactDamage <= 0f) return;
        float dist = Vector2.Distance(player.position, rb.position);
        if (dist <= BodyRadius + playerRadius)
            DamagePlayer(contactDamage);
    }

    private void ChangeScarcity(float amount)
    {
        Scarcity = Mathf.Clamp(Scarcity + amount, 0f, maxScarcity);
        OnScarcityChanged?.Invoke(Scarcity, maxScarcity);

        RemnantPhase newPhase = CalculatePhase();
        if (newPhase != Phase)
        {
            Phase = newPhase;
            Debug.Log($"[Remnant] Phase changed to {Phase}");
            OnPhaseChanged?.Invoke(Phase);
        }

        UpdatePlayerVulnerability();
    }

    private RemnantPhase CalculatePhase()
    {
        if (Scarcity >= ravenousAbove) return RemnantPhase.Ravenous;
        if (Scarcity < weakenedBelow) return RemnantPhase.Weakened;
        return RemnantPhase.Stable;
    }

    //the hungrier the boss, the more damage the player takes
    private void UpdatePlayerVulnerability()
    {
        if (playerStats == null || !IsActiveOrStarting) return;
        playerStats.DamageTakenMultiplier = Mathf.Lerp(playerDamageMultiplierMin, playerDamageMultiplierMax, Scarcity01);
    }

    private bool IsActiveOrStarting => State != RemnantState.Transformed;

    private void HandlePlayerDied()
    {
        playerDead = true;
        if (IsActive) EnterState(RemnantState.Recover);
    }



    private void UpdateVisuals()
    {
        if (body == null) return;
        ApplyBodyScale(false);

        if (bodyRenderer == null) return;

        Color baseColor;
        if (State == RemnantState.Dormant) baseColor = dormantColor;
        else if (State == RemnantState.Transformed) baseColor = transformedColor;
        else if (State == RemnantState.Transforming)
            baseColor = Color.Lerp(transformedColor, PhaseColor(), stateDuration > 0f ? stateTimer / stateDuration : 0f);
        else baseColor = PhaseColor();

        //with the golem art keep the sprite's own colors and only add a light hint for dormant, ravenous and transformed
        if (!tintBodyByPhase)
        {
            if (State == RemnantState.Dormant) baseColor = Color.Lerp(Color.white, dormantColor, 0.5f);
            else if (State == RemnantState.Transformed) baseColor = Color.Lerp(Color.white, transformedColor, 0.6f);
            else if (State == RemnantState.Transforming) baseColor = Color.Lerp(Color.white, transformedColor, 0.6f * StateProgress01);
            else if (Phase == RemnantPhase.Ravenous) baseColor = Color.Lerp(Color.white, ravenousColor, 0.35f);
            else baseColor = Color.white;
        }

        //blink while winding up an attack so it's readable even without the ground marker.
        bool windingUp = State == RemnantState.TelegraphSlam || State == RemnantState.TelegraphLunge;
        if (windingUp && Mathf.FloorToInt(Time.time * 12f) % 2 == 0)
            baseColor = Color.Lerp(baseColor, Color.white, 0.5f);

        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;
            baseColor = Color.Lerp(baseColor, flashColor, flashTimer / FlashDuration);
        }

        bodyRenderer.color = baseColor;
    }

    private void ApplyBodyScale(bool instant)
    {
        float target = Mathf.Lerp(minScale, maxScale, Scarcity01);

        if (State == RemnantState.Dormant) target = minScale * 0.5f;
        else if (State == RemnantState.Transforming || State == RemnantState.Transformed) target = minScale * 0.6f;

        currentScale = instant ? target : Mathf.Lerp(currentScale, target, scaleChangeSpeed * Time.deltaTime);
        punch = Mathf.Lerp(punch, 0f, 8f * Time.deltaTime);

        float s = Mathf.Max(0.05f, currentScale * (1f + punch));
        body.localScale = new Vector3(s, s, 1f);
    }

    private Color PhaseColor()
    {
        if (Phase == RemnantPhase.Weakened) return weakenedColor;
        if (Phase == RemnantPhase.Ravenous) return ravenousColor;
        return stableColor;
    }

    private void Flash(Color c)
    {
        flashColor = c;
        flashTimer = FlashDuration;
    }

    private void UpdateSlamTelegraph()
    {
        if (slamTelegraph == null) return;
        slamTelegraph.transform.position = transform.position;

        //grows from the center and gets more solid as the hit gets closer.
        float progress = stateDuration > 0f ? 1f - stateTimer / stateDuration : 1f;
        float d = SlamRadius * 2f * Mathf.Lerp(0.3f, 1f, progress);
        SetTelegraphSize(slamTelegraph, d, d);
        slamTelegraph.color = new Color(telegraphColor.r, telegraphColor.g, telegraphColor.b,
                                        telegraphColor.a * Mathf.Lerp(0.3f, 1f, progress));
    }

    private void UpdateLungeTelegraph()
    {
        if (lungeTelegraph == null) return;

        float length = lungeSpeed * lungeDuration + BodyRadius;
        Vector2 start = rb.position;
        Vector2 mid = start + lungeDirection * (length * 0.5f);

        lungeTelegraph.transform.position = new Vector3(mid.x, mid.y, transform.position.z);
        float angle = Mathf.Atan2(lungeDirection.y, lungeDirection.x) * Mathf.Rad2Deg;
        lungeTelegraph.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        SetTelegraphSize(lungeTelegraph, length, lungeLaneWidth * Mathf.Max(1f, currentScale * 0.8f));

        float progress = stateDuration > 0f ? 1f - stateTimer / stateDuration : 1f;
        lungeTelegraph.color = new Color(telegraphColor.r, telegraphColor.g, telegraphColor.b,
                                         telegraphColor.a * Mathf.Lerp(0.25f, 1f, progress));
    }

    //sizes a telegraph sprite in world units, whatever size the sprite file is
    private static void SetTelegraphSize(SpriteRenderer sr, float width, float height)
    {
        if (sr.sprite == null) return;
        Vector2 spriteSize = sr.sprite.bounds.size;
        if (spriteSize.x <= 0f || spriteSize.y <= 0f) return;
        sr.transform.localScale = new Vector3(width / spriteSize.x, height / spriteSize.y, 1f);
    }

    private void HideTelegraphs()
    {
        if (slamTelegraph != null) slamTelegraph.enabled = false;
        if (lungeTelegraph != null) lungeTelegraph.enabled = false;
    }

    private void SetVelocity(Vector2 v)
    {
        if (rb == null) return;
#if UNITY_6000_0_OR_NEWER
        rb.linearVelocity = v;
#else
        rb.velocity = v;
#endif
    }


    private void OnDrawGizmosSelected()
    {
        float scale = Application.isPlaying ? currentScale : Mathf.Lerp(minScale, maxScale, startScarcity / Mathf.Max(1f, maxScarcity));
        Vector3 p = transform.position;

        Gizmos.color = new Color(1f, 0.3f, 0.1f, 0.8f);
        Gizmos.DrawWireSphere(p, slamRadius * scale); //slam area

        Gizmos.color = new Color(1f, 1f, 0.2f, 0.8f);
        Gizmos.DrawWireSphere(p, bodyRadius * scale); //body / contact

        Gizmos.color = new Color(0.3f, 0.7f, 1f, 0.5f);
        Gizmos.DrawWireSphere(p, lungeMinDistance); //lunges from beyond this

#if UNITY_EDITOR
        if (Application.isPlaying)
            UnityEditor.Handles.Label(p + Vector3.up * (slamRadius * scale + 0.3f),
                $"{State}\n{Phase}  scarcity {Scarcity:0}");
#endif
    }




}