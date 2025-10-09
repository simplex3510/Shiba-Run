using UnityEngine;
using Manager;

public class MenuButton : MonoBehaviour
{
    public void OnClickStart()
    {
        GameSceneManager.Instance.ActivatePreloadedMainGameScene();
    }

    public void OnClickCredit()
    {
        GameSceneManager.Instance.LoadScene(GameSceneManager.SceneNames.CreditScene);
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
