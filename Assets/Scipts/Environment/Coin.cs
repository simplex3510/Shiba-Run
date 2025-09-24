using UnityEngine;
using Manager;

public enum CoinTypes : int
{
    Bronze = 0,
    Silver,
    Gold,

    Size,
}

[RequireComponent(typeof(Animator))]
public class Coin : MonoBehaviour
{
    [SerializeField]
    private CoinTypes coinType;

    private Animator animator;

    private int coinTypeId;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        coinTypeId = Animator.StringToHash("CoinType");
    }

    private void Start()
    {
        int beCoin = Random.Range(0, 2);
        if (0 < beCoin)
        {
            int whatType = Random.Range(1, 101);
            if (whatType <= 60)
            {
                // 60%
                coinType = CoinTypes.Bronze;
            }
            else if (whatType <= 90)
            {
                // 30%
                coinType = CoinTypes.Silver;
            }
            else
            {
                // 10%
                coinType = CoinTypes.Gold;
            }
        }

        coinType = (CoinTypes)Random.Range(0, (int)CoinTypes.Size);
        animator.SetInteger(coinTypeId, (int)coinType);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.AddCoinScore(coinType);
            Destroy(gameObject);
        }
    }
}
