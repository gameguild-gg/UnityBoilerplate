using System.Threading;
using UnityEngine;

public class NPC_Flee_State : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] int fleeSpeed;
    [SerializeField] GameObject fleeFrom;
    [SerializeField] int safeDistance;
    [Tooltip("The thing that the NPC is fleeing from")]

    public void Flee()
    {
        if (fleeFrom == null) return;

        float distance = Vector3.Distance(transform.position, fleeFrom.transform.position);

        if (distance < safeDistance)
        {
            Vector3 direction = transform.position - fleeFrom.transform.position;
            direction.Normalize();

            transform.position += direction * fleeSpeed * Time.deltaTime;
        }
    }

    public void setFleeFrom(GameObject FleeFrom)
    {
        fleeFrom = FleeFrom;
    }

}
