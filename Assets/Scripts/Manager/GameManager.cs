using UnityEngine;
using Singleton;
using TMPro;
using System.ComponentModel;

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

        #region Game State
        public bool IsGameStarted { get { return isGameStarted; } set { isGameStarted = value; } }
        public bool IsGameOver { get { return isGameOver; } set { isGameOver = value; } }

        [SerializeField, ReadOnlyField] private bool isGameOver = false;
        
        [SerializeField, ReadOnlyField] private bool isGameStarted = false;
        #endregion

        #region Game UI
        private TextMeshProUGUI scoreText;
        #endregion


        [SerializeField]
        private float waitTime = 3.0f;

        private void Awake()
        {
            
        }

        private void Start()
        {
            float randomSeed = System.DateTime.Now.Ticks;
            Random.InitState((int)randomSeed);
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
                        Score += Time.deltaTime * 1.5f;
                        break;
                    case GamePhases.FastPhase:
                        Score += Time.deltaTime * 3f;
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
                scoreText.text = $"Score: {Mathf.Round(Score)}";
            }
            else
            {
                Debug.LogWarning("ScoreText is not allocated.");
            }
        }
        #endregion
    }
}