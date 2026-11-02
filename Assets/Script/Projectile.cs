using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float speed)
    {
        rb.linearVelocity = (direction * speed);
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); 
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }

}