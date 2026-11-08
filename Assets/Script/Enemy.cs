using UnityEngine;

///
/// Enemy behavior: sleeps until the player gets close (or attacks them). Once disturbed, they wake up, 
/// start chasing the player.
///

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private bool isInvincible;
    private float invincibleCount;
    [SerializeField] private float detectionRadius = 5f; 
    [SerializeField] private float speed = 4f;      
    [SerializeField] private int strength = 5;           
    private Transform playerPosition;
    private bool isChasing = false;
    [SerializeField] private AudioSource roarSound;
    [SerializeField] private AudioSource snoreSound;
    [SerializeField] private float roarInterval = 10f;
    private float roarTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // Find the player object in the scene by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerPosition = playerObj.transform;
        }
        // start playing the snoring sound in a loop when the enemy is still idle
        if (!snoreSound.isPlaying)
        {
            snoreSound.Play();
        }
    }

    void Update()
    {
        // temporary invincibility cooldown after taking damage
        if (isInvincible)
        {
            invincibleCount += Time.deltaTime;
            if (invincibleCount > 1f)
            {
                isInvincible = false;
                invincibleCount = 0;
            }
        }
        // when the enemy is chasing the player
        if (isChasing)
        {
            // Stop snoring
            if (snoreSound.isPlaying)
            {
                snoreSound.Stop();
            }
            // Periodically play the roar sound during the chase using timer
            roarTimer -= Time.deltaTime;
            if (roarTimer <= 0f)
            {
                roarSound.Play();
                roarTimer = roarInterval;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if hit by a player projectile
        if (collision.gameObject.GetComponent<Projectile>() != null)
        {
            rb.linearVelocity = Vector2.zero;
            Hit();
        }
    }

    private void Hit()
    {
        // take damage if not invincible
        if (!isInvincible)
        {
            strength--;
            // wake up and start chasing when hit
            isChasing = true;

            // destroy enemy if health drops to zero or less
            if (strength <= 0)
            {
                Destroy(this.gameObject);
                return;
            }
            isInvincible = true;
        }
    }

    private void FixedUpdate()
    {
        //calculate distance between enemy and player
        if (playerPosition == null) return;
            float distanceToPlayer = Vector2.Distance(rb.transform.position, playerPosition.position);

        //trigger chase mode if the player enters the detection radius
        if (distanceToPlayer <= detectionRadius)
            isChasing = true;

        if (isChasing)
        {
            // Determine movement direction towards the player
            float direction = Mathf.Sign(playerPosition.position.x - rb.transform.position.x);

            Vector2 position = rb.transform.position;
            position.x += direction * speed * Time.fixedDeltaTime;
            rb.transform.position = position;
            // Update animator parameters for movement
            animator.SetInteger("Direction", (int)direction);
            animator.SetFloat("Move", direction);
        }
        else
        {
            //idle state
            animator.SetFloat("Move", 0);
        }
    }
}
