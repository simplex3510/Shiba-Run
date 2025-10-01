using UnityEngine;
using Manager;

public class BackgroundLoop : MonoBehaviour
{
    private float speed;

    private float scaleFactor;
    private float width;


    private void Awake()
    {
        scaleFactor = transform.localScale.x;
        width = GetComponent<SpriteRenderer>().size.x * scaleFactor;
    }

    private void Start()
    {
        speed = PlatformManager.Instance.Speed;
    }

    private void Update()
    {
        if (transform.position.x <= -width)
        {
            Reposition();
        }
    }

    private void Reposition()
    {
        Vector2 offset = new Vector2(width * 2, 0);
        transform.position = (Vector2)transform.position + offset;
    }
}
