using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    bool isHit = false;
    [SerializeField] private Sprite inactive;
    [SerializeField] private Sprite active;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (inactive != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = inactive;
        }
    }

    public void Hit()
    {
        if (!isHit)
        {
            isHit = true;

            if (active != null && spriteRenderer != null)
            {
                spriteRenderer.sprite = active;
            }
        }
    }
}