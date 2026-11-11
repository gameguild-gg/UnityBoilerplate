using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class NPC_Patrolling : MonoBehaviour
{
    [SerializeField] int patrolSpeed =3;
    [SerializeField] int currentTarget=0;
    [Tooltip("The current target that the NPC is moving towards")]
    [SerializeField] Transform[] targets;
    [Tooltip("A list of all targets the NPC should move from")]

    public float reachThreshold = 0.2f; // Distance to consider waypoint reached

    public void Patrol()
    {
        if (targets.Length == 0) return;

        Transform targetWaypoint = targets[currentTarget];

        // Move towards the current waypoint
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetWaypoint.position,
            patrolSpeed * Time.deltaTime
        );

        // Check if close enough to the waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) <= reachThreshold)
        {
            currentTarget = (currentTarget + 1) % targets.Length;
        }
    }

}
