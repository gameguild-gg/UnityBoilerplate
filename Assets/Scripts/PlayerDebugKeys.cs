using UnityEngine;
using UnityEngine.InputSystem;

//Debug keys for testing PlayerStats
//1 = State Meter -1 2 = State Meter +10
//3 = Take 1 damage  4 = Heal 1
//5 = Kill player
public class PlayerDebugKeys : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private float stateMeterStep = 10f;
    [SerializeField] private float damageStep = 1f;

    private void Awake()
    {
        if (stats == null)
        {
            Debug.LogError("PlayerDebugKeys: Drag the Player's PlayerStats into the 'Stats' slot.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        //it is in the true in the Editor and in Development Builds, false in normal builds.
        if (!Debug.isDebugBuild) return;

        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        if (kb.digit1Key.wasPressedThisFrame) stats.ChangeStateMeter(-stateMeterStep);
        if (kb.digit2Key.wasPressedThisFrame) stats.ChangeStateMeter(stateMeterStep);
        if (kb.digit3Key.wasPressedThisFrame) stats.TakeDamage(damageStep);
        if (kb.digit4Key.wasPressedThisFrame) stats.Heal(damageStep);
        if (kb.digit5Key.wasPressedThisFrame) stats.Kill();
    }
}
