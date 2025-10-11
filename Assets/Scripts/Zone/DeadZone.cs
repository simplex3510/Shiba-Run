using Manager;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);

            GameManager.Instance.SetGameOver();

            SoundManager.Instance.StopBGM();
            SoundManager.Instance.PlaySFX(AudioClipNames.GameSet);
        }
    }
}
