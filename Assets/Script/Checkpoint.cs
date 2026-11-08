using UnityEngine;

/// 
/// Checkpoint logic: starts inactive, and when the player touches it, 
/// it switches to an active sprite, plays a sound effect, and saves the players position
///

public class Checkpoint : MonoBehaviour
{
    // Flag to check if this checkpoint has already been activated
    bool isHit = false;
    [SerializeField] private Sprite inactive;
    [SerializeField] private Sprite active;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioSource checkpointSound;

    void Start()
    {
        // get the SpriteRenderer component to change the checkpoint's  sprite
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Set the initial sprite to inactive one when the game starts
        if (inactive != null)
        {
            spriteRenderer.sprite = inactive;
        }
    }
    // called when player triggers the checkpoint
    public void Hit()
    {
        if (!isHit)
        {
            isHit = true;
            checkpointSound.Play();
         // Switch the sprite to the active one
            if (active != null)
            {
                spriteRenderer.sprite = active;
            }
        }
    }
}