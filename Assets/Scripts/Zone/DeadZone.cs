using Manager;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.SetGameOver();

            SoundManager.Instance.StopBGM();
            SoundManager.Instance.PlaySFX(AudioClipNames.GameSet);
        }
    }
}
