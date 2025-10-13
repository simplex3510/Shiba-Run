using UnityEngine;
using Manager;

public class MoveBackground : MonoBehaviour
{
    private float speed;
    private float width;

    private Vector2 startPosition;

    private void Awake()
    {
        float scaleFactor = transform.localScale.x;
        width = GetComponent<SpriteRenderer>().size.x * scaleFactor;

        startPosition = transform.position;

        speed = PlatformManager.Instance.Speed;

        GameManager.Instance.OnInitializeGame += Initialize;
    }

    private void OnEnable()
    {
        transform.position = startPosition;        
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsGameStarted == false || GameManager.Instance.IsGameOver == true)
            return;

        transform.Translate(speed * Time.fixedDeltaTime * Vector2.left);
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

    public void Initialize()
    {
        transform.position = startPosition;
    }
}
