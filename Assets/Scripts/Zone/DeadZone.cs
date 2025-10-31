using Manager;
using UnityEngine;
using UnityEngine.UI;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);

            GameManager.Instance.SetGameOver();
        }
    }
}
