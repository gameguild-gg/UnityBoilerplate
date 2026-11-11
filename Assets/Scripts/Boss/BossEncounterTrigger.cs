
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BossEncounterTrigger : MonoBehaviour
{
    [SerializeField] private ScarcityRemnant remnant;
    [SerializeField] private PlayerStats playerStats;
    [Tooltip("Optional: objects (like door blockers) turned ON when the fight starts and OFF when it's won.")]
    [SerializeField] private GameObject[] lockObjects;

    private bool triggered;

    private void Awake()
    {
        if (remnant == null || playerStats == null)
        {
            Debug.LogError("BossEncounterTrigger need to drag in the remenat and rhe player stats");
            enabled = false;
            return;
        }
        SetLocks(false);
    }

    private void OnEnable()
    {
        if (remnant != null) remnant.OnTransformed += HandleTransformed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!enabled || triggered) return;

        //only react to the player we were given
        if (other.GetComponentInParent<PlayerStats>() != playerStats) return;

        triggered = true;
        SetLocks(true);
        remnant.StartEncounter();
    }

    private void HandleTransformed() => SetLocks(false);

    private void SetLocks(bool on)
    {
        if (lockObjects == null) return;

        foreach (GameObject go in lockObjects)
            if (go != null) go.SetActive(on);
    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
