using UnityEngine;

using Manager;
using TMPro;

public class ScoreText : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.Instance.AllocateScoreUI(GetComponent<TextMeshProUGUI>());
    }
}
