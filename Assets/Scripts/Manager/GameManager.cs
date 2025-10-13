using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Singleton;
using System.Threading.Tasks;

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
        public delegate void OnInitialize();
        public OnInitialize OnInitializeGame;

        public GamePhases GamePhase { get; private set; } = GamePhases.SlowPhase;

        public float Score { get; private set; } = 0.0f;

        #region Menu UI
        [Header("Menu UI")]
        public bool IsClickedStartButton { get { return isClickedStartButton; } }
        [SerializeField, ReadOnlyField] private bool isClickedStartButton = false;
        #endregion

        #region Game UI
        [Header("Game UI")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private GameObject titleButton;
        #endregion

        #region Game State
        [Header("Game State")]
        public bool IsGameStarted { get { return isGameStarted; } }
        public bool IsGameOver { get { return isGameOver; } }

        [SerializeField, ReadOnlyField] private bool isGameOver = false;
        [SerializeField, ReadOnlyField] private bool isGameStarted = false;
        #endregion

        [SerializeField] private float waitTime = 3.0f;

        public PlayerInputActions inputActions;

        #region Unity Callbacks
        private void Awake()
        {
            // Input System 초기화
            inputActions = new PlayerInputActions();
            inputActions.Dev.Enable();

            // ESC (Pause 액션) 눌렀을 때 종료
            inputActions.Dev.Exit.performed += _ => QuitGame();
        }

        private void Start()
        {
            OnInitializeGame += Initialize;

            int seed = System.Environment.TickCount;
            Random.InitState(seed);
        }

        private void Update()
        {
            if (!isClickedStartButton)
                return;

            if (!IsGameOver)
            {
                if (!IsGameStarted)
                {
                    waitTime -= Time.deltaTime;
                    if (waitTime < 0.0f)
                    {
                        isGameStarted = true;
                        Debug.Log("Game Started");
                    }
                }
                else
                {
                    AddTimeScore();

                    UpdateScoreUI();
                }
            }
        }

        private void OnDestroy()
        {
            inputActions.Dev.Exit.performed -= _ => QuitGame();
            inputActions.Dev.Disable();
        }
        #endregion

        public void Initialize()
        {

            isClickedStartButton = false;
            isGameOver = false;
            isGameStarted = false;
            waitTime = 3.0f;
            Score = 0.0f;

            titleButton.SetActive(false);
        }

        public async void SetGameOver()
        {
            isGameOver = true;

            SoundManager.Instance.StopBGM();
            float delayTime = SoundManager.Instance.PlaySFX(AudioClipNames.GameSet);

            await Task.Delay((int)(delayTime * 1000));

            titleButton.SetActive(true);
        }

        private void QuitGame()
        {
#if UNITY_EDITOR
            // 에디터에서 테스트 시
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // 빌드 시
            Application.Quit();
#endif
            Debug.Log("Game exited (alpha version).");
        }

        #region Menu Button CallBacks
        public void OnClickStartButton()
        {
            OnInitializeGame?.Invoke();

            isClickedStartButton = true;
        }

        public void OnClickExitButton()
        {
            QuitGame();
        }
        #endregion

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
                Debug.LogError("ScoreText is not allocated.");
            }
        }
        #endregion

    }
}