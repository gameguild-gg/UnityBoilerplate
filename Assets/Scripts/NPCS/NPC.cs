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

    [SerializeField] NPC_States currentState= NPC_States.DEFAULT;
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
                Debug.Log("RUH ROH");
                return;
                
            case NPC_States.DEFAULT:
                DefaultState();
                return;

            case NPC_States.CAUTIOUS:
                CautiousState();
                return;

            case NPC_States.FLEEING:
                canTalk = false;
                fleeState.Flee();
                return;

            case NPC_States.PATROLLING:
                patrolState.Patrol();
                return;

            case NPC_States.FOLLOWING:
                canTalk = false;
                followState.Follow();
                return;
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
        Debug.Log(other);
        if(other.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement player))
        {
            playerInRange = true;
            DialogueCanvas.Instance.UpdateText(interactionDialogue.dialogue); //instance isnt working

        }

        if(other.gameObject == followState.dropOffPoint)
        {
            Debug.Log("DROP OFF NPC");
            currentState= NPC_States.DEFAULT;
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
