using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 move;
    private Rigidbody2D rb;
    private int direction = 1;
    private Animator animator;
    public float jumpHeight = 2.4f;
    private bool isJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        move = InputSystem.actions["Move"].ReadValue<Vector2>();

        if (InputSystem.actions["Jump"].IsPressed() && isJumping == false)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(new Vector2(0, Mathf.Sqrt(-2 * Physics2D.gravity.y * jumpHeight)), ForceMode2D.Impulse);
            isJumping = true;
        }

        if (move.x != 0)
        {
            direction = move.x < 0 ? -1 : 1;
            animator.SetInteger("Direction", direction);
        }
        animator.SetFloat("Move", move.x);
    }

    void FixedUpdate()
    {
        Vector2 position = rb.transform.position;
        position.x += (move.x * speed) * Time.fixedDeltaTime;
        rb.transform.position = position;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
        }
    }
}
