using UnityEngine;
using Manager;
using System.Collections.Generic;

public class MovePlatform : MonoBehaviour
{
    private float speed;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        speed = PlatformManager.Instance.Speed;
    }

    private void FixedUpdate()
    {
        if (!PlatformManager.Instance.CanMovePlatform)
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
