using UnityEngine;

public class NPC_Flee_State : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] int fleeSpeed;
    [SerializeField] GameObject fleeFrom;
    [SerializeField] CircleCollider2D circleCollider;
    [Tooltip("The thing that the NPC is fleeing from")]

    public void Flee()
    {

    }
}
