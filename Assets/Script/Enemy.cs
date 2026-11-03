using UnityEngine;

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
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerPosition = playerObj.transform;
        }
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibleCount += Time.deltaTime;
            if (invincibleCount > 1f)
            {
                isInvincible = false;
                invincibleCount = 0;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() != null)
        {
            rb.linearVelocity = Vector2.zero;
            Hit();
        }
    }

    private void Hit()
    {
        if (!isInvincible)
        {
            strength--;
            isChasing = true;
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
        if (playerPosition == null) return;
            float distanceToPlayer = Vector2.Distance(rb.transform.position, playerPosition.position); 
        

        if (distanceToPlayer <= detectionRadius)
            isChasing = true;

        if (isChasing)
        {
            float direction = Mathf.Sign(playerPosition.position.x - rb.transform.position.x);

            Vector2 position = rb.transform.position;
            position.x += direction * speed * Time.fixedDeltaTime;
            rb.transform.position = position;

            animator.SetInteger("Direction", (int)direction);
                animator.SetFloat("Move", direction);
        }
        else
        {
                animator.SetFloat("Move", 0);
        }
    }
}
