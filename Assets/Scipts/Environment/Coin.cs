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
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            spriteRenderer.enabled = false;
            return;
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

        spriteRenderer.enabled = true;
        animator.SetInteger("CoinType", (int)type);
    }

    private int GetScoreByType()
    {
        switch (type)
        {
            case CoinTypes.None:
                return 0;
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

            gameObject.SetActive(false);
        }
    }
}
