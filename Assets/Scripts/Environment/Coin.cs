using UnityEngine;
using Manager;
using AnimParams;

public enum CoinTypes : int
{
    None = 0,

    Bronze,
    Silver,
    Gold,

    Size,
}

[RequireComponent(typeof(Animator))]
public class Coin : MonoBehaviour
{
    public AnimIntParam CoinType { get; private set; }

    private Animator animator;

    [SerializeField] private CoinTypes type;

    private const int BRONZE_COIN_SCORE = 50;
    private const int SILVER_COIN_SCORE = 100;
    private const int GOLD_COIN_SCORE = 150;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        CoinType = new AnimIntParam(animator, "CoinType");
    }

    private void OnEnable()
    {
        SetCoinType();
    }

    private void SetCoinType()
    {
        // 백분률 계산
        int whatType = Random.Range(0, 100) + 1;

        // 40%
        if (whatType <= 40)
        {
            type = CoinTypes.Bronze;
        }
        // 35%
        else if (whatType <= 75)
        {
            type = CoinTypes.Silver;
        }
        // 25%
        else
        {
            type = CoinTypes.Gold;
        }

        animator.SetInteger("CoinType", (int)type);
    }

    private int GetScoreByType()
    {
        switch (type)
        {
            case CoinTypes.Bronze:
                return BRONZE_COIN_SCORE;
            case CoinTypes.Silver:
                return SILVER_COIN_SCORE;
            case CoinTypes.Gold:
                return GOLD_COIN_SCORE;
        }

        return -1;
    }

    // 플레이어가 코인과 충돌했을 때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int coinScore = GetScoreByType();
            if (coinScore < 0)
            {
                Debug.LogError("Error: Does Not Add Score by Coin Type");
                return;
            }
            GameManager.Instance.AddScore(coinScore);

            SoundManager.Instance.PlaySFX(AudioClipNames.Coin);

            gameObject.SetActive(false);
        }
    }
}
