using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] Player player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.gameObject.CompareTag("Heart") && collision.gameObject.CompareTag("Player"))
        {
            player.AddHp();
            Destroy(this.gameObject);
        }
    }
}
