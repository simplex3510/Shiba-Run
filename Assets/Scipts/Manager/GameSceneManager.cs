using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using Singleton;

namespace Manager
{
    public class GameSceneManager : SingletonBase<GameSceneManager>
    {
        public enum SceneNames
        {
            MainMenuScene = 0,
            MainGameScene,
            CreditScene,
        }

        public bool IsLoadedMainGameScene { get; private set; } = false;

        private AsyncOperation preloadMenuOperation = null;
        private AsyncOperation preloadGameOperation = null;

        private void Start()
        {
            StartCoroutine(PreloadSceneAsync(SceneNames.MainGameScene, LoadSceneMode.Additive, false));
            SceneManager.LoadScene((int)SceneNames.MainMenuScene);
        }

        private IEnumerator PreloadSceneAsync(SceneNames sceneName, LoadSceneMode mode, bool activateOnLoad = true)
        {
            // additive 모드로 로드 (현재 씬 유지)
            AsyncOperation preloadOperation = SceneManager.LoadSceneAsync((int)sceneName, mode);

            //즉시 활성화 방지
            preloadOperation.allowSceneActivation = activateOnLoad;

            switch (sceneName)
            {
                case SceneNames.MainMenuScene:
                    preloadMenuOperation = preloadOperation;
                    break;

                case SceneNames.MainGameScene:
                    preloadGameOperation = preloadOperation;
                    break;
            }

            while (!preloadOperation.isDone)
            {
                yield return null;
            }

            

            yield break;
        }


        public void ShowScene(SceneNames sceneIndex)
        {
            HideScene();

            Scene scene = SceneManager.GetSceneByBuildIndex((int)sceneIndex);
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                if (root.activeInHierarchy == false)
                {
                    root.SetActive(true);
                }
            }
        }

        public void ShowMainGameScene()
        {
            HideScene();

            Scene scene = SceneManager.GetSceneByBuildIndex((int)SceneNames.MainGameScene);
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                if (root.activeInHierarchy == false)
                {
                    root.SetActive(true);
                }
            }

            IsLoadedMainGameScene = true;
        }

        private void HideScene(SceneNames sceneIndex)
        {
            Scene scene = SceneManager.GetSceneByBuildIndex((int)sceneIndex);
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                root.SetActive(false);
            }
        }

        private void HideScene()
        {
            Scene scene = SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex);
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                root.SetActive(false);
            }
        }


        public void LoadScene(SceneNames scene)
        {
            SceneManager.LoadScene((int)scene);
        }

        public void ReloadCurrentScene()
        {
            Scene current = SceneManager.GetActiveScene();
            SceneManager.LoadScene(current.buildIndex);
        }

        public void ActivatePreloadedMainGameScene()
        {
            if (preloadOperation == null)
                return;

            HideScene();

            preloadOperation.allowSceneActivation = true;

            IsLoadedMainGameScene = true;
        }
    }
}
