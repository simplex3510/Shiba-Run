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

    [SerializeField] private Animator animator;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private CoinTypes type;

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
        // MEMO: NONE 타입일 때 애니메이션 처리
        SetCoinType();
    }

    private void SetCoinType()
    {
        // 백분률 계산
        int whatType = Random.Range(0, 100) + 1;

        // 35%
        if (whatType <= 35)
        {
            type = CoinTypes.None;
        }
        // 30%
        else if (whatType <= 60)
        {
            type = CoinTypes.Bronze;
        }
        // 20%
        else if (whatType <= 80)
        {
            type = CoinTypes.Silver;
        }
        // 15%
        else
        {
            type = CoinTypes.Gold;
        }

        animator.SetInteger("CoinType", (int)type);
        //CoinType.IntValue = (int)type;
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
}
