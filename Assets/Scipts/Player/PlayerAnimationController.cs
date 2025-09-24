using System.Collections;

using UnityEngine;
using Manager;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private PlayerController playerController;
    private Animator animator;
    private int isGroundId;
    private int isWalkId;
    private int isRunId;
    private int jumpTriggerId;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();

        // 문자열 → int hash 변환
        isGroundId = Animator.StringToHash("IsGround");
        isWalkId = Animator.StringToHash("IsWalk");
        isRunId = Animator.StringToHash("IsRun");
        jumpTriggerId = Animator.StringToHash("JumpTrigger");
    }

    private void Start()
    {
        StartCoroutine(WaitGameStart());
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameStarted == false &&
            GameManager.Instance.IsGameOver == true)
            return;

        CheckGround();
    }

    private IEnumerator WaitGameStart()
    {
        while (!GameManager.Instance.IsGameStarted)
            yield return null;

        ToggleMoveMode();

        yield break;
    }

    private void CheckGround()
    {
        if (playerController.IsGround)
        {
            animator.SetBool(isGroundId, true);
        }
        else
        {
            animator.SetBool(isGroundId, false);
            animator.SetTrigger(jumpTriggerId);
        }
    }

    private void CheckScore()
    {

    }
    
    private void ToggleMoveMode()
    {
        bool walk = animator.GetBool(isWalkId);
        bool run = animator.GetBool(isRunId);

        // Initialize
        if (walk == false && run == false)
        {
            animator.SetBool(isWalkId, true);
            animator.SetBool(isRunId, false);
            return;
        }

        // Toggle
        if (walk == true && run == false)
        {
            animator.SetBool(isWalkId, false);
            animator.SetBool(isRunId, true);
        }
        else
        {
            animator.SetBool(isWalkId, true);
            animator.SetBool(isRunId, false);
        }
    }
}