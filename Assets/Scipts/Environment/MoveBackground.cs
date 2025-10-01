using UnityEngine;
using Manager;

public class MoveBackground : MonoBehaviour
{
    private float speed;

    private void Start()
    {
        speed = PlatformManager.Instance.Speed;
    }

    private void FixedUpdate()
    {
        if (!PlatformManager.Instance.CanMovePlatform)
            return;

        transform.Translate(Vector2.left * speed * Time.fixedDeltaTime);
    }
}
