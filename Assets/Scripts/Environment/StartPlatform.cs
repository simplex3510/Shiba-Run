using UnityEngine;

using Manager;

public class StartPlatform : MovePlatform
{
    private Vector2 initialPosition;

    protected override void Awake()
    {
        base.Awake();
        
        initialPosition = transform.position;

        GameManager.Instance.OnInitializeGame += Initialize;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }

    public void Initialize()
    {
        transform.position = initialPosition;
        gameObject.SetActive(true);
    }
}
