using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathLayerDetector : MonoBehaviour
{
    public LayerMask deathLayer; // Assign the "Death" layer in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is the player
        if (other.gameObject.CompareTag("Player"))
        {
            // Reset the player's position
            BallReset ballReset = other.gameObject.GetComponent<BallReset>();
            ballReset.ResetBallPosition();
        }
    }
}
    