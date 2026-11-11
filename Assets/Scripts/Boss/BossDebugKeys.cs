
using UnityEngine;
using UnityEngine.InputSystem;

public class BossDebugKeys : MonoBehaviour
{
    [SerializeField] private ScarcityRemnant remnant;
    [Tooltip("How strong each debug action is (1 = one normal attack / help).")]
    [SerializeField] private float strength = 1f;

    private void Awake()
    {
        if(remnant == null)
        {
            Debug.LogError("BossDebugkes drage the bosses scarcity renmant into rnemant feild");
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Debug.isDebugBuild) return;

        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        if (kb.digit7Key.wasPressedThisFrame) remnant.StartEncounter();

        if (kb.digit8Key.wasPressedThisFrame) remnant.ReceiveAggression(strength);

        if (kb.digit9Key.wasPressedThisFrame) remnant.ReceiveHelp(strength);

        if (kb.digit0Key.wasPressedThisFrame) remnant.ForceTransform();

    }
}
