using System.Collections;

using UnityEngine;
using Manager;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    public Animator PlayerAnimator { get; private set; }

    private PlayerController playerController;
    private int isGroundId;
    private int walkTriggerId;
    private int runTriggerId;
    private int jumpTriggerId;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        PlayerAnimator = GetComponent<Animator>();

        // 문자열 → int hash 변환
        isGroundId = Animator.StringToHash("IsGround");
        walkTriggerId = Animator.StringToHash("WalkTrigger");
        runTriggerId = Animator.StringToHash("RunTrigger");
        jumpTriggerId = Animator.StringToHash("JumpTrigger");
    }

    private void Start()
    {
        StartCoroutine(WaitGameStart());
    }

    private void Update()
    {
        Debug.Log($"IsGameStarted: {GameManager.Instance.IsGameStarted}");
        Debug.Log($"IsGameOver: {GameManager.Instance.IsGameOver}");

        if (GameManager.Instance.IsGameStarted == false ||
            GameManager.Instance.IsGameOver == true)
            return;

        Debug.Log("Update Called");

        CheckGround();
    }

    public void SetJumpTrigger()
    {
        PlayerAnimator.SetTrigger(jumpTriggerId);
    }

    private IEnumerator WaitGameStart()
    {
        while (GameManager.Instance.IsGameStarted == false)
            yield return null;

        PlayerAnimator.SetTrigger(walkTriggerId);

        yield break;
    }

    private void CheckGround()
    {
        if (playerController.IsGround == true)
        {
            PlayerAnimator.SetBool(isGroundId, true);

            
        }
        else
        {
            PlayerAnimator.SetBool(isGroundId, false);
        }
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            switch (GameManager.Instance.GamePhase)
            {
                case GamePhases.SlowPhase:
                    PlayerAnimator.SetTrigger(walkTriggerId);
                    break;
                case GamePhases.FastPhase:
                    PlayerAnimator.SetTrigger(runTriggerId);
                    break;
            }
        }
    }
}