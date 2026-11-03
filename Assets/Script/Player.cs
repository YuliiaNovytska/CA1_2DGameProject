using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
    public int hp = 5;
    public float speed = 5f;
    private Vector2 move;
    private Rigidbody2D rb;
    private int direction = 1;
    private Animator animator;
    public float jumpHeight = 2.4f;
    private bool isJumping = false;
    private int gold = 0;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI hpText;
    public GameObject projectilePrefab;
    public float cooldown = 5f;
    private float cooldownTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        UpdateHp();
    }


    void Update()
    {
        move = InputSystem.actions["Move"].ReadValue<Vector2>();

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

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
        animator.SetFloat("Move", Mathf.Abs(move.x));

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Fire();
        }

        if (isJumping && Keyboard.current.leftShiftKey.isPressed && cooldownTimer <= 0)
        {
            rb.linearDamping = 7f;
            rb.gravityScale = 0;
            animator.SetBool("Freeze", true);

            if (Camera.main != null)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

                Vector2 direction = mousePos - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }
        else
        {
            rb.gravityScale = 3f;
            rb.linearDamping = 0f;
            animator.SetBool("Freeze", false);

            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rb.transform.position;
        if (isJumping && Keyboard.current.leftShiftKey.isPressed)
        {
            
            position.x += (0f * speed) * Time.fixedDeltaTime;
        }
        else
        {
            position.x += (move.x * speed) * Time.fixedDeltaTime;
        }
        rb.transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            hp--;
            UpdateHp();

            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            hp--;
            UpdateHp();

            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void AddCoin()
    {
        gold++;
        if (goldText != null)
        {
            goldText.text = "" + gold;
        }
    }

    public void UpdateHp()
    {
        if (hpText != null)
        {
            hpText.text = "" + hp;
        }
    }

    private void Fire()
    {
        Vector2 shootDirection;
        float force = 2f;

        if (isJumping && Keyboard.current.leftShiftKey.isPressed && cooldownTimer <= 0)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            shootDirection = (mousePos - transform.position).normalized;
            force = 50f;
            cooldownTimer = cooldown;
        }

        else
        {
            shootDirection = new Vector2(direction, 0);
            
        }

        GameObject go = Instantiate(projectilePrefab, rb.transform.position + Vector3.up * 0.5f, Quaternion.identity);
        Projectile pro = go.GetComponent<Projectile>();
        pro.Launch(shootDirection, 30f);

        
        rb.AddForce(-shootDirection * force, ForceMode2D.Impulse);
    }
}
