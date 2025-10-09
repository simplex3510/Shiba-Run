using UnityEngine;
using Singleton;
using TMPro;

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
        public bool IsGameStarted { get; private set; } = false;
        public bool IsGameOver { get; private set; } = false;

        #region Game UI
        private TextMeshProUGUI scoreText;
        #endregion


        [SerializeField]
        private float waitTime = 3.0f;

        private void Awake()
        {
            float randomSeed = System.DateTime.Now.Ticks;
            Random.InitState((int)randomSeed);
        }

        private void Start()
        {

        }

        private void Update()
        {
            if (!GameSceneManager.Instance.IsLoadedMainGameScene)
                return;

            if (!IsGameOver)
            {
                if (!IsGameStarted)
                {
                    waitTime -= Time.deltaTime;
                    if (waitTime < 0.0f)
                    {
                        IsGameStarted = true;
                    }
                }
                else
                {
                    AddTimeScore();

                    UpdateScoreUI();
                }
            }
        }

        public void SetGameOver()
        {
            IsGameOver = true;
        }

        #region Score Method
        public void AddScore(int score)
        {
            Score += score;
        }

        public void AddScore(float score)
        {
            Score += score;
        }

        private void AddTimeScore()
        {
            if (IsGameStarted && !IsGameOver)
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

        #region Score UI
        public bool AllocateScoreUI(TextMeshProUGUI scoreText)
        {
            if (this.scoreText == null)
            {
                this.scoreText = scoreText;
                return true;
            }
            return false;
        }

        private void UpdateScoreUI()
        {
            if (scoreText)
            {
                scoreText.text = $"Score: {Score:0}";
            }
            else
            {
                Debug.LogWarning("ScoreText is not allocated.");
            }
        }
        #endregion
    }
}