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

        [Header("Reposition Setting")]
        [SerializeField] private float maxHeight;
        [SerializeField] private float minHeight;
        [SerializeField] private float repositionX;

        void Start()
        {
            StartCoroutine(WaitGameStart());
        }

        private void Update()
        {
            if (!CanMovePlatform)
                return;
        }

        public void RepositionPlatform(GameObject platform)
        {
            float randomY = Random.Range(minHeight, maxHeight);
            platform.transform.position = new Vector2(repositionX, randomY);
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