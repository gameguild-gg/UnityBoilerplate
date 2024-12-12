using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReset : MonoBehaviour
{
    public Vector3 resetPosition = new Vector3(0f, 0f, 0f);

    public void ResetBallPosition()
    {
        transform.position = resetPosition;
    }
}
