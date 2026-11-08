using UnityEngine;

public class Heart : MonoBehaviour
{
    // Reference to the Player script to interact with player health
    [SerializeField] Player player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object is tagged as Heart and the colliding object is Player
        if (this.gameObject.CompareTag("Heart") && collision.gameObject.CompareTag("Player"))
        {
            // Increase the player's hp
            player.AddHp();
            // Destroy the heart from the scene after it is collected
            Destroy(this.gameObject);
        }
    }
}
