using UnityEngine;
using Manager;

public class MoveBackground : MonoBehaviour
{
    private float speed;
    private float width;

    private void Awake()
    {
        float scaleFactor = transform.localScale.x;
        width = GetComponent<SpriteRenderer>().size.x * scaleFactor;
    }

    private void Start()
    {
        speed = PlatformManager.Instance.Speed;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsGameStarted == false || GameManager.Instance.IsGameOver == true)
            return;

        transform.Translate(Vector2.left * speed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        if (-width < transform.position.x)
            return;
        
        Reposition();
    }

        private void Reposition()
    {
        Vector2 offset = new Vector2(width * 2, 0);
        transform.position = (Vector2)transform.position + offset;
    }
}
