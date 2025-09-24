using System.Collections;

using UnityEngine;
using Singleton;

namespace Manager
{
    public class EnvironmentManager : SingletonBase<EnvironmentManager>
    {
        public float Speed { get { return speed; } }


        [SerializeField]
        private float speed = 5.0f;

        private bool isGameStarted = false;

        void Start()
        {
            StartCoroutine(WaitGameStart());
        }

        private void Update()
        {
            if (!isGameStarted)
                return;
        }

        private IEnumerator WaitGameStart()
        {
            while (!GameManager.Instance.IsGameStarted)
                yield return null;

            isGameStarted = true;

            yield break;
        }
    }
}