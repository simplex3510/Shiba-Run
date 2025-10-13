using UnityEngine;

using Manager;

public class MainGameUICanvas : MonoBehaviour
{
    private void OnEnable()
    {
        if (SoundManager.Instance.Initialized)
        {
            SoundManager.Instance.PlayBGM(AudioClipNames.MainGameBGM);
        }
        else
        {
            StartCoroutine(StartBGM());
        }
    }

    private System.Collections.IEnumerator StartBGM()
    {
        while (!SoundManager.Instance.Initialized)
        {
            yield return null;
        }

        SoundManager.Instance.PlayBGM(AudioClipNames.MainGameBGM);
        yield break;
    }
}
