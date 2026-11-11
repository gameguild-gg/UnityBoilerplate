using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Input")]
    [Tooltip("Drag PlayerControls.inputactions here. Needs a 'Player' map with 'Move' and 'Sprint' actions.")]
    [SerializeField] private InputActionAsset inputActions;

    [Header("Speeds (units per second)")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 6.5f;

    [Header("Feel")]
    [Tooltip("How fast the player reaches full speed. Higher = snappier.")]
    [SerializeField] private float acceleration = 50f;
    [Tooltip("How fast the player stops when you let go. Lower = more slide (Isaac has a little).")]
    [SerializeField] private float deceleration = 35f;
    [Tooltip("Stick values below this are ignored (prevents drift on controllers).")]
    [SerializeField, Range(0f, 0.5f)] private float stickDeadzone = 0.2f;

    [Header("Optional")]
    [Tooltip("Sprite to flip left/right based on movement. Leave empty to skip.")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Tooltip("Lets other scripts (stun, cutscenes, dialogue) turn movement off.")]
    [SerializeField] private bool canMove = true;

    public Vector2 MoveInput { get; private set; }
    public Vector2 FacingDirection { get; private set; } = Vector2.down;
    public bool IsSprinting { get; private set; }
    public bool IsMoving => MoveInput.sqrMagnitude > 0.01f;
    public Vector2 CurrentVelocity => GetVelocity();

    public bool CanMove
    {
        get => canMove;
        set => canMove = value;
    }

    //Multiplier other systems can change e.g. slow when State Meter is low, or a hidden enemy is "attached"
    [HideInInspector] public float speedMultiplier = 1f;

    private Rigidbody2D rb;
    private InputAction moveAction;
    private InputAction sprintAction;
    private bool createdActionsInCode;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        //Top-down setup: no gravity, no spinning when bumping walls.
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        SetUpInput();
    }

    private void SetUpInput()
    {
        if (inputActions != null)
        {
            //Use the actions from the PlayerControls asset.
            moveAction = inputActions.FindAction("Player/Move", throwIfNotFound: true);
            sprintAction = inputActions.FindAction("Player/Sprint", throwIfNotFound: true);
            createdActionsInCode = false;
            return;
        }

        Debug.LogWarning($"{name}: No Input Actions asset assigned on PlayerMovement, using built-in default bindings.", this);
        CreateFallbackInputActions();
        createdActionsInCode = true;
    }

    private void CreateFallbackInputActions()
    {
        //WASD, arrow keys, and gamepad left stick.
        moveAction = new InputAction("Move", InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        moveAction.AddBinding("<Gamepad>/leftStick");

        //hold Left Shift, or press the left stick / left trigger on a gamepad.
        sprintAction = new InputAction("Sprint", InputActionType.Button);
        sprintAction.AddBinding("<Keyboard>/leftShift");
        sprintAction.AddBinding("<Gamepad>/leftStickPress");
        sprintAction.AddBinding("<Gamepad>/leftTrigger");
    }

    private void OnEnable()
    {
        moveAction.Enable();
        sprintAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        sprintAction.Disable();
    }

    private void OnDestroy()
    {
        //Only dispose actions this script created itself never the shared asset's actions
        if (createdActionsInCode)
        {
            moveAction?.Dispose();
            sprintAction?.Dispose();
        }
    }

    private void Update()
    {
        //Read input every frame.
        Vector2 raw = canMove ? moveAction.ReadValue<Vector2>() : Vector2.zero;

        if (raw.magnitude < stickDeadzone)
            raw = Vector2.zero;

        //Clamp so diagonals (W + D) are the same speed as straight lines,
        //but a half-pushed stick still walks slower.
        MoveInput = Vector2.ClampMagnitude(raw, 1f);

        IsSprinting = IsMoving && sprintAction.IsPressed();

        if (IsMoving)
            FacingDirection = MoveInput.normalized;

        //Flip sprite left/right.
        if (spriteRenderer != null && Mathf.Abs(MoveInput.x) > 0.01f)
            spriteRenderer.flipX = MoveInput.x < 0f;
    }

    private void FixedUpdate()
    {
        float speed = (IsSprinting ? sprintSpeed : walkSpeed) * speedMultiplier;
        Vector2 targetVelocity = MoveInput * speed;

        //Speed up quickly when moving, slow down a bit softer when stopping.
        float rate = IsMoving ? acceleration : deceleration;
        Vector2 newVelocity = Vector2.MoveTowards(GetVelocity(), targetVelocity, rate * Time.fixedDeltaTime);

        SetVelocity(newVelocity);
    }

    //Instantly stops the player useful for knockback resets, cutscenes, etc.
    public void StopImmediately()
    {
        SetVelocity(Vector2.zero);
    }

    private Vector2 GetVelocity()
    {
        if (rb == null) return Vector2.zero;
#if UNITY_6000_0_OR_NEWER
        return rb.linearVelocity;
#else
        return rb.velocity;
#endif
    }

    private void SetVelocity(Vector2 v)
    {
#if UNITY_6000_0_OR_NEWER
        rb.linearVelocity = v;
#else
        rb.velocity = v;
#endif
    }
}
