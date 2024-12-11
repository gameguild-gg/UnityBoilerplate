using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    // Reference to score
    private ScoreManager _scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var ball = other.gameObject.GetComponent<BallMove>();
        // If the other is the ball
        if (ball != null)
        {
            _scoreManager.Score++;
            Destroy(gameObject);
        }
    }
}
 