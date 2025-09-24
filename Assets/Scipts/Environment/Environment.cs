using UnityEngine;
using Manager;

public class Environment : MonoBehaviour
{
    private float speed;

    private void Start()
    {
        speed = EnvironmentManager.Instance.Speed;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameStarted == false ||
            GameManager.Instance.IsGameOver == true)
            return;

        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
