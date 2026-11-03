using UnityEngine;

public class NPC : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    enum NPC_States
    {
        NONE,
        DEFAULT,
        CAUTIOUS,
        FLEEING,
        PATROLLING,
        FOLLOWING,
    }

    [SerializeField] NPC_States currentState;
    [Tooltip("The current state that the NPC is in.")]

    [SerializeField] bool canTalk;
    [Tooltip("Determines if the player can currently talk to this NPC")]
    [SerializeField] Dialogue interactionDialogue;
    [Tooltip("What the NPC says when interacted with")]


    private void FixedUpdate()
    {
        SwitchStates();
    }

    private void SwitchStates()
    {
        switch (currentState)
        {
            case NPC_States.NONE:
                break;

            case NPC_States.DEFAULT:
                DefaultState();
                break;

            case NPC_States.CAUTIOUS:
                break;

            case NPC_States.FLEEING:
                break;

            case NPC_States.PATROLLING:
                break;

            case NPC_States.FOLLOWING:
                break;
        }
    }

    private void DefaultState()
    {
        canTalk =true;
    }

    public void Talk() //have this be called when player presses E in the NPCS interact radius
    {
        DialogueCanvas.Instance.UpdateText(interactionDialogue.dialogue);
    }
}
