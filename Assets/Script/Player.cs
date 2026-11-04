using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
//player stats
    public int hp = 5;
    public float speed = 5f;
    private Vector2 move;
    private Rigidbody2D rb;
    private int direction = 1;
    public float jumpHeight = 2.4f;
    private bool isJumping = false;
    private int gold = 0;
//UI
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI hpText;
//Projectile prefab
    public GameObject projectilePrefab;
//settings for cooldown when "Freeze"
    public float cooldown = 5f;
    private float cooldownTimer = 0f;
//Animator
    private Animator animator;

    void Start()
    {
//getting components when the game starts + updating lives
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        UpdateHp();
    }

    void Update()
    {
    //read movement
        move = InputSystem.actions["Move"].ReadValue<Vector2>();
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    //let thr jump happen if the button is pressed and the player is on the ground (not jumping yet)
        if (InputSystem.actions["Jump"].IsPressed() && isJumping == false)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(new Vector2(0, Mathf.Sqrt(-2 * Physics2D.gravity.y * jumpHeight)), ForceMode2D.Impulse);
    //showing that the jump is happening
            isJumping = true;
        }
    //change direction and updating animator when the player moves left/right
        if (move.x != 0)
        {
            direction = move.x < 0 ? -1 : 1;
            animator.SetInteger("Direction", direction);
        }
        animator.SetFloat("Move", Mathf.Abs(move.x));
    //F key for "Fire"
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Fire();
        }
    //freeze in the air when player is jumping, left shift is pressed and cooldown is ready
        if (isJumping && Keyboard.current.leftShiftKey.isPressed && cooldownTimer <= 0)
        {
    //linearDamping adds some kind of air resistance, so the movement gets smoother before player freezes
            rb.linearDamping = 7f;
            rb.gravityScale = 0;
            animator.SetBool("Freeze", true);
    //if the main camera there, letting the sprite rotate around itself towards the cursor of the mouse
            if (Camera.main != null)
            {
    //Mouse.current.position.ReadValue() gives the curren coordinates of the position of the mouse
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                Vector2 direction = mousePos - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }
        else
        {
    //if not frozen, changing to the original gravity and rotation
            rb.gravityScale = 3f;
            rb.linearDamping = 0f;
            animator.SetBool("Freeze", false);
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }

    void FixedUpdate()
    {
    //handling movement, blockinf when player is frozen in the air
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
    //when player is on the ground, letting them to jump again
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
        }
    //when touching an enemy, decreasing healt and udating the UI of it
        if (collision.gameObject.tag == "Enemy")
        {
            hp--;
            UpdateHp();
    //when there is no more lives, destroying the player
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    //when it's a trap (using the same "Enemy" tag, but it is a Trigger)
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
    //increase the coin count and updating the UI for coins
    public void AddCoin()
    {
        gold++;
        if (goldText != null)
        {
            goldText.text = "" + gold;
        }
    }
    //updating the UI for lives
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
    //if the player is frozen, shoot in the direction of a mouse
        if (isJumping && Keyboard.current.leftShiftKey.isPressed && cooldownTimer <= 0)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            shootDirection = (mousePos - transform.position).normalized;
    //making the force stronger
            force = 50f;
    // trigger cooldown, so the player can't shoot too often
            cooldownTimer = cooldown;
        }
    //if not frozen, shoot in the normal facing direction
        else
        {
            shootDirection = new Vector2(direction, 0);
        }
    //spawn the bullet and launch it
        GameObject go = Instantiate(projectilePrefab, rb.transform.position + Vector3.up * 0.5f, Quaternion.identity);
        Projectile pro = go.GetComponent<Projectile>();
        pro.Launch(shootDirection, 30f);
        
    //push the player in the opposit direction
        rb.AddForce(-shootDirection * force, ForceMode2D.Impulse);
    }
}
