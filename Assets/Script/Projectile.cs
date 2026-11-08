using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    //launch the projectile in a given direction with a specific speed
    public void Launch(Vector2 direction, float speed)
    {
        // Set the velocity of the projectile
        rb.linearVelocity = (direction * speed);
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // destroy projectile immediately when colliding with any object
        Destroy(this.gameObject);
    }

}