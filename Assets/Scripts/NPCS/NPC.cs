using UnityEngine;
using UnityEngine.InputSystem;

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

    [SerializeField] bool canTalk=false;
    [Tooltip("Determines if the player can currently talk to this NPC")]
    [SerializeField] Dialogue interactionDialogue;
    [Tooltip("What the NPC says when interacted with")]
    [SerializeField] bool playerInRange=false;
    [Tooltip("Checks if the player is in range to interact with the NPC")]

    [SerializeField] private InputActionAsset inputActions;

    [Header("Other State Scripts")]
    [SerializeField] NPC_Flee_State fleeState;
    [SerializeField] NPC_Patrolling patrolState;
    [SerializeField] NPC_Follow followState;

    private void Start()
    {
        if(this.gameObject.TryGetComponent<NPC_Flee_State>(out NPC_Flee_State flee))
        {
            fleeState=flee;
        }

        if (this.gameObject.TryGetComponent<NPC_Patrolling>(out NPC_Patrolling patrol))
        {
            patrolState = patrol;
        }

        if (this.gameObject.TryGetComponent<NPC_Follow>(out NPC_Follow follow))
        {
            followState = follow;
        }

    }

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
                CautiousState();
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

    private void CautiousState()
    {
        canTalk=false;
    }
    public void Talk() //have this be called when player presses E in the NPCS interact radius
    {
        if(playerInRange &&
            canTalk)
        {
            DialogueCanvas.Instance.UpdateText(interactionDialogue.dialogue);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement player))
        {
            playerInRange = true;
            DialogueCanvas.Instance.UpdateText(interactionDialogue.dialogue); //instance isnt working

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement player))
        {
            playerInRange = false;
        }
    }

}
