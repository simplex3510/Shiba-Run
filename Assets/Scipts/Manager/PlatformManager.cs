using System.Collections;

using UnityEngine;
using Singleton;
using Unity.Collections;

namespace Manager
{
    public class PlatformManager : SingletonBase<PlatformManager>
    {
        public float Speed { get { return speed; } }
        public bool CanMovePlatform { get; private set; } = false;

        [Header("Move Setting")]
        [SerializeField] private float speed = 5.0f;



        void Start()
        {
            StartCoroutine(WaitGameStart());
        }

        private void Update()
        {
            if (!CanMovePlatform)
                return;
        }

        private IEnumerator WaitGameStart()
        {
            while (!GameManager.Instance.IsGameStarted)
                yield return null;

            CanMovePlatform = true;

            yield break;
        }

    }
}