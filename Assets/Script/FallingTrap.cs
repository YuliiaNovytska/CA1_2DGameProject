using UnityEngine;

public class FallingTrap : MonoBehaviour
{
    private Rigidbody2D rb;

    private bool isFalling = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFalling && collision.CompareTag("Player"))
        {
            isFalling = true;
            rb.gravityScale = 3f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
