using UnityEngine;
using Manager;

public class MovePlatform : MonoBehaviour
{
    private float speed;
    private Vector2 velocity;

    private Rigidbody2D rb;

    private Coin[] coins;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coins = GetComponentsInChildren<Coin>();
    }

    protected virtual void OnEnable()
    {
        foreach (var coin in coins)
        {
            coin.gameObject.SetActive(true);
        }
    }

    protected virtual void Start()
    {
        speed = PlatformManager.Instance.Speed;
        velocity = Vector2.left * speed;
    }

    protected virtual void FixedUpdate()
    {
        if (GameManager.Instance.IsGameStarted == true)
        {
            if (GameManager.Instance.IsGameOver == true)
            {
                rb.linearVelocity = Vector2.down * speed;
                return;
            }
            else
            {
                rb.linearVelocity = velocity;
                return;
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("DeadZone"))
        {
            rb.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("RepositionZone"))
        {
            rb.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
