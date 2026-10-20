using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] Player player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.tag == "Coin" && collision.gameObject.tag == "Player")
        {
            player.AddCoin();
            Destroy(this.gameObject);
        }
    }
}
