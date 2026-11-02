using UnityEngine;

public class BossSpriteAnimator : MonoBehaviour
{
    [System.Serializable]
    public class Clip
    {
        public Sprite[] frames;
        public float fps = 12f;
        public bool loop = true;
        public bool HasFrames => frames != null && frames.Length > 0;
    }

    [Header("References")]
    [SerializeField] private ScarcityRemnant remnant;
    [Tooltip("The SpriteRenderer on Body > Sprite (the same one ScarcityRemnant tints).")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Facing")]
    [Tooltip("Tick if the art faces RIGHT in the sprite sheet. Untick if it faces LEFT.")]
    [SerializeField] private bool artFacesRight = true;

    [Header("Looping clips")]
    [SerializeField] private Clip idle = new Clip { fps = 10f, loop = true };
    [SerializeField] private Clip walk = new Clip { fps = 12f, loop = true };
    [Tooltip("Played during the lunge charge (Run works well).")]
    [SerializeField] private Clip lungeCharge = new Clip { fps = 18f, loop = true };

    [Header("Slam (GroundSlam)")]
    [SerializeField] private Clip slam = new Clip { fps = 16f, loop = false };
    [Tooltip("Frame index (starting at 0) where the slam hits the ground. Frames before it = wind-up.")]
    [SerializeField] private int slamImpactFrame = 8;

    [Header("Lunge wind-up")]
    [Tooltip("Played (stretched) during the lunge warning. An attack or crouch works well.")]
    [SerializeField] private Clip lungeWindup = new Clip { fps = 12f, loop = false };

    [Header("Reactions")]
    [Tooltip("Played when the boss is attacked and gets fed (Cast / roar works well).")]
    [SerializeField] private Clip feeding = new Clip { fps = 16f, loop = false };
    [Tooltip("Played when a helpful action staggers the boss (Stunned or GetHit).")]
    [SerializeField] private Clip weakened = new Clip { fps = 16f, loop = false };

    [Header("Encounter start / end")]
    [Tooltip("Played (stretched) while waking up. Land or Cast works well.")]
    [SerializeField] private Clip awaken = new Clip { fps = 12f, loop = false };
    [Tooltip("Played (stretched) while transforming. Die works well.")]
    [SerializeField] private Clip transform_ = new Clip { fps = 12f, loop = false };
    [Tooltip("Shown while Dormant. Leave empty to show Idle frame 0, frozen.")]
    [SerializeField] private Clip dormant = new Clip { fps = 4f, loop = true };
    [Tooltip("Shown after the transformation. Leave empty to hold the last frame of Transform.")]
    [SerializeField] private Clip transformed = new Clip { fps = 8f, loop = true };

    private Clip current;
    private float clipTime;
    private bool flipped;

    private void Awake()
    {
        if (remnant == null || spriteRenderer == null)
        {
            Debug.LogError("BossSpriteAnimator: Drag in the Remnant and the boss Sprite Renderer.", this);
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (remnant != null) remnant.OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        if (remnant != null) remnant.OnStateChanged -= HandleStateChanged;
    }

    private void Start()
    {
        if (remnant != null) HandleStateChanged(remnant.State);
    }

    private void HandleStateChanged(RemnantState state)
    {
        clipTime = 0f;
        current = ClipFor(state);
    }

    //LateUpdate so ScarcityRemnant has already updated this frame.
    private void LateUpdate()
    {
        if (remnant == null || spriteRenderer == null) return;

        RemnantState state = remnant.State;
        clipTime += Time.deltaTime;

        UpdateFacing(state);

        Sprite frame = null;
        switch (state)
        {
            case RemnantState.TelegraphSlam:
                //Wind-up frames [0, impact) stretched over the warning.
                if (slam.HasFrames)
                    frame = FrameByProgress(slam, 0, Mathf.Clamp(slamImpactFrame, 1, slam.frames.Length), remnant.StateProgress01);
                break;

            case RemnantState.Slam:
                //Impact frame onward at normal speed.
                if (slam.HasFrames)
                {
                    int start = Mathf.Clamp(slamImpactFrame, 0, slam.frames.Length - 1);
                    int i = start + Mathf.FloorToInt(clipTime * slam.fps);
                    frame = slam.frames[Mathf.Min(i, slam.frames.Length - 1)];
                }
                break;

            case RemnantState.TelegraphLunge:
            case RemnantState.Awakening:
            case RemnantState.Transforming:
                if (current != null && current.HasFrames)
                    frame = FrameByProgress(current, 0, current.frames.Length, remnant.StateProgress01);
                break;

            case RemnantState.Dormant:
                if (dormant.HasFrames) frame = FrameByTime(dormant);
                else if (idle.HasFrames) frame = idle.frames[0];
                break;

            case RemnantState.Transformed:
                if (transformed.HasFrames) frame = FrameByTime(transformed);
                else if (transform_.HasFrames) frame = transform_.frames[transform_.frames.Length - 1];
                break;

            default:
                if (current != null && current.HasFrames) frame = FrameByTime(current);
                break;
        }

        //Fallback: Idle, so nothing ever goes blank.
        if (frame == null && idle.HasFrames) frame = FrameByTime(idle);
        if (frame != null) spriteRenderer.sprite = frame;
    }

    private Clip ClipFor(RemnantState state)
    {
        Clip c;
        switch (state)
        {
            case RemnantState.Stalking: c = walk; break;
            case RemnantState.TelegraphSlam:
            case RemnantState.Slam: c = slam; break;
            case RemnantState.TelegraphLunge: c = lungeWindup; break;
            case RemnantState.Lunge: c = lungeCharge.HasFrames ? lungeCharge : walk; break;
            case RemnantState.Feeding: c = feeding; break;
            case RemnantState.Weakening: c = weakened; break;
            case RemnantState.Awakening: c = awaken; break;
            case RemnantState.Transforming: c = transform_; break;
            case RemnantState.Transformed: c = transformed; break;
            case RemnantState.Dormant: c = dormant; break;
            default: c = idle; break; // Recover
        }
        return c.HasFrames ? c : idle;
    }

    private Sprite FrameByTime(Clip clip)
    {
        int i = Mathf.FloorToInt(clipTime * Mathf.Max(0.01f, clip.fps));
        i = clip.loop ? i % clip.frames.Length : Mathf.Min(i, clip.frames.Length - 1);
        return clip.frames[i];
    }

    private static Sprite FrameByProgress(Clip clip, int from, int toExclusive, float progress01)
    {
        int count = Mathf.Max(1, toExclusive - from);
        int i = from + Mathf.Min(count - 1, Mathf.FloorToInt(progress01 * count));
        return clip.frames[Mathf.Clamp(i, 0, clip.frames.Length - 1)];
    }

    private void UpdateFacing(RemnantState state)
    {
        //Don't turn around in the middle of an attack.
        bool locked = state == RemnantState.TelegraphSlam || state == RemnantState.Slam
                   || state == RemnantState.Transforming || state == RemnantState.Transformed
                   || state == RemnantState.Dormant;

        if (!locked)
        {
            float dirX;
            if (state == RemnantState.TelegraphLunge || state == RemnantState.Lunge)
                dirX = remnant.LungeDirection.x;
            else
                dirX = remnant.Player != null ? remnant.Player.position.x - remnant.transform.position.x : 0f;

            if (Mathf.Abs(dirX) > 0.05f)
            {
                bool facingLeft = dirX < 0f;
                flipped = artFacesRight ? facingLeft : !facingLeft;
            }
        }

        spriteRenderer.flipX = flipped;
    }

}
