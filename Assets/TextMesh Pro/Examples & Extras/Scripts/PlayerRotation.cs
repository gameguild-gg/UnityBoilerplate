using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class PlayerRotation : MonoBehaviour
{
    public float rotationAmount = 45f;

    IEnumerator DelayedFunctionCall(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            StartCoroutine(DelayedFunctionCall(3f));
            transform.Rotate(0, -rotationAmount, 0);

        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, rotationAmount, 0);
            Thread.Sleep(125);
        }
    }
}
