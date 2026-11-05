using UnityEngine;

public class NPC_Follow : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;

    [SerializeField] int followSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //follow speed should be equal to or slightly less than player speen
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
