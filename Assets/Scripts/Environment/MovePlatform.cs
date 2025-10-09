using UnityEngine;
using Manager;

public class MovePlatform : MonoBehaviour
{
    private float speed;

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
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsGameStarted == false || GameManager.Instance.IsGameOver == true)
            return;

        rb.MovePosition(rb.position + Vector2.left * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("RepositionZone"))
        {
            this.gameObject.SetActive(false);
        }
    }
}
