using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    bool isHit = false;
    [SerializeField] private Sprite inactive;
    [SerializeField] private Sprite active;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    [SerializeField] private AudioSource checkpointSound;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (inactive != null)
        {
            spriteRenderer.sprite = inactive;
        }
    }

    public void Hit()
    {
        if (!isHit)
        {
            isHit = true;
            checkpointSound.Play();

            if (active != null)
            {
                spriteRenderer.sprite = active;
            }
        }
    }
}