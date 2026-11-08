using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] Player player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //checking if the object is tagged as Coin and the colliding one is Player
        if (this.tag == "Coin" && collision.gameObject.tag == "Player")
        {
            // Increase players coin count
            player.AddCoin();
            //Destroy the coin object after it is collected
            Destroy(this.gameObject);
        }
    }
}
