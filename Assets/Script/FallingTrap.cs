using UnityEngine;

public class FallingTrap : MonoBehaviour
{
    private Rigidbody2D rb;

    // flag to ensure the trap only falls once when triggered
    private bool isFalling = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Disable gravity at the start so the trap stays floating in place
        rb.gravityScale = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the trap hasn't fallen yet and if the object entering is Player
        if (!isFalling && collision.CompareTag("Player"))
        {
            // Marking trap as falling so this trigger only fires once
            isFalling = true;
            //Turn on gravity so the trap drops down
            rb.gravityScale = 3f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //destroying the trap upon colliding with anything (ground, walls, or the player)
        Destroy(gameObject);
    }
}
