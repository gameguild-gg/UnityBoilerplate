using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//When the player's health hits 0: stop movement, tint the sprite, wait, then reload the room
public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private PlayerMovement movement;          
    [SerializeField] private SpriteRenderer spriteRenderer;  
    [SerializeField] private float restartDelay = 1.5f;
    [SerializeField] private Color deadTint = new Color(1f, 0.3f, 0.3f, 0.6f);

    private bool restarting;

    private void OnEnable()
    {
        if (stats == null)
        {
            Debug.LogError("PlayerDeath: Drag the Player's PlayerStats into the 'Stats' slot.", this);
            enabled = false;
            return;
        }
        stats.OnDied += HandleDied;
    }

    private void OnDisable()
    {
        if (stats != null) stats.OnDied -= HandleDied;
    }

    private void HandleDied()
    {
        if (restarting) return;
        restarting = true;

        if (movement != null)
        {
            movement.CanMove = false;
            movement.StopImmediately();
        }

        if (spriteRenderer != null)
            spriteRenderer.color = deadTint;

        Debug.Log("Player died. Restarting room...");
        StartCoroutine(RestartAfterDelay());
    }

    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(restartDelay);

        Scene scene = SceneManager.GetActiveScene();
        if (scene.buildIndex >= 0)
            SceneManager.LoadScene(scene.buildIndex);
        else
            Debug.LogError($"Can't restart: scene '{scene.name}' is not in the Build Profiles / Build Settings scene list. Add it there.");
    }
}
