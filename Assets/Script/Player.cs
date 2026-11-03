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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        UpdateHp();
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
        animator.SetFloat("Move", Mathf.Abs(move.x));

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Fire();
        }
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
        GameObject go = Instantiate(projectilePrefab, rb.transform.position + Vector3.up * 0.5f, Quaternion.identity);
        Projectile pro = go.GetComponent<Projectile>();
        pro.Launch(new Vector2(direction, 0), 10f);
    }
}
