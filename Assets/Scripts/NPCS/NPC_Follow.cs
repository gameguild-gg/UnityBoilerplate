using UnityEngine;

public class NPC_Follow : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] int followSpeed;
    [SerializeField] int seperationDistance;
    [SerializeField ] public GameObject dropOffPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public void Follow()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > seperationDistance)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.transform.position,
                followSpeed * Time.deltaTime
            );
        }
    }

  
}
