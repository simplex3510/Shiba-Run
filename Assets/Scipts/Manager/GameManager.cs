using UnityEngine;
using Singleton;

namespace Manager
{
    public enum GamePhases : int
    {
        None = 0,

        SlowPhase,
        FastPhase,

        Size,
    }

    public class GameManager : SingletonBase<GameManager>
    {
        public GamePhases GamePhase { get; private set; } = GamePhases.SlowPhase;

        public float Score { get; private set; } = 0.0f;
        public bool IsCompletedCameraMove { get; set; } = false;
        public bool IsGameStarted { get; private set; } = false;
        public bool IsGameOver { get; private set; } = false;


        [SerializeField]
        private float waitTime = 3.0f;

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
                    Score += 30.0f;
                    break;
                case CoinTypes.Silver:
                    Score += 60.0f;
                    break;
                case CoinTypes.Gold:
                    Score += 100.0f;
                    break;
            }
        }

        private void AddTimeScore()
        {
            if (IsGameOver && IsGameStarted)
            {
                switch (GamePhase)
                {
                    case GamePhases.SlowPhase:
                        Score += Time.deltaTime;
                        break;
                    case GamePhases.FastPhase:
                        Score += Time.deltaTime * 1.5f;
                        break;
                }
            }
        }
        #endregion

        private void OnPlayerOver()
        {
            IsGameOver = true;
        }
    }
}