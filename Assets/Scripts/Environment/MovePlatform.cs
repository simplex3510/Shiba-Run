using UnityEngine;
using Manager;

public class MovePlatform : MonoBehaviour
{
    private float speed;
    private Vector2 velocity;

    private Rigidbody2D rb;

    private Coin[] coins;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coins = GetComponentsInChildren<Coin>();
    }

    private void OnEnable()
    {
        foreach (var coin in coins)
        {
            coin.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        speed = PlatformManager.Instance.Speed;
        velocity = Vector2.left * speed;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsGameStarted == false || GameManager.Instance.IsGameOver == true)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = velocity;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("RepositionZone"))
        {
            this.gameObject.SetActive(false);
        }
    }
}
