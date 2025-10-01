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

    private SpriteRenderer spriteRenderer;

    private CoinTypes type;

    private const int BRONZE_COIN_SCORE = 30;
    private const int SILVER_COIN_SCORE = 50;
    private const int GOLD_COIN_SCORE = 100;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        CoinType = new AnimIntParam(animator, "CoinType");
    }

    private void OnEnable()
    {
        SetCoinType();
    }

    private void OnDisable()
    {
        SetCoinType();
    }

    private void SetCoinType()
    {
        CoinTypes randomType = (CoinTypes)Random.Range((int)CoinTypes.Bronze, (int)CoinTypes.Size);
        if (randomType == CoinTypes.None)
        {
            type = CoinTypes.None;
        }
        else
        {
            // 확률 계산
            int whatType = Random.Range(0, 100) + 1;

            // 60% 확률
            if (whatType <= 60)
                type = CoinTypes.Bronze;
            // 30% 확률
            else if (whatType <= 90)
                type = CoinTypes.Silver;
            // 10% 확률
            else
                type = CoinTypes.Gold;
        }

        CoinType.IntValue = Random.Range((int)CoinTypes.None, (int)CoinTypes.Size);
    }

    private int GetScoreByType()
    {
        int score = 0;

        switch (type)
        {
            case CoinTypes.Bronze:
                score = BRONZE_COIN_SCORE;
                break;
            case CoinTypes.Silver:
                score = SILVER_COIN_SCORE;
                break;
            case CoinTypes.Gold:
                score = GOLD_COIN_SCORE;
                break;

            default:
                Debug.LogError("Error: Undefined Coin Type");
                return -1;
        }

        return score;
    }


    // 플레이어가 코인과 충돌했을 때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int coinScore = GetScoreByType();
            GameManager.Instance.AddScore(coinScore);

            gameObject.SetActive(false);
        }
    }

    // 플레이어가 코인과 충돌하지 않고 지나쳐갈 경우, RepositionZone에서 코인 재설정.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Zone"))
        {
            SetCoinType();
        }
    }
}
