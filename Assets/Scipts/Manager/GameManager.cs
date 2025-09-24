using UnityEngine;
using Singleton;

namespace Manager
{
    public class GameManager : SingletonBase<GameManager>
    {
        public float GetScore { get { return score; } }
        public bool IsCompletedCameraMove { get; set; } = false;
        public bool IsGameStarted { get; private set; } = false;
        public bool IsGameOver { get; private set; } = false;

        [SerializeField]
        private float waitTime = 3.0f;

        private float score = 0.0f;

        private void Awake()
        {

        }

        private void Start()
        {

        }

        private void Update()
        {
            if (IsGameStarted)
            {
                AddTimeScore();
            }
            else
            {
                waitTime -= Time.deltaTime;
                if (waitTime <= 0.0f)
                {
                    IsGameStarted = true;
                }
            }
        }

#region Score Method
        public void AddCoinScore(CoinTypes coinTypes)
        {
            switch (coinTypes)
            {
                case CoinTypes.Bronze:
                    score += 30.0f;
                    break;
                case CoinTypes.Silver:
                    score += 60.0f;
                    break;
                case CoinTypes.Gold:
                    score += 100.0f;
                    break;
            }
        }
        
        private void AddTimeScore()
        {
            if (IsGameOver && IsGameStarted)
            {
                score += Time.deltaTime;
            }
        }
#endregion

        private void OnPlayerOver()
        {
            IsGameOver = true;
        }
    }
}