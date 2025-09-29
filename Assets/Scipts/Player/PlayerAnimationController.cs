using System.Collections;

using UnityEngine;
using Manager;
using Unity.Mathematics;

abstract class AnimParam
{
    protected Animator animator;
    protected int id;

    protected AnimParam(Animator animator, string name)
    {
        this.animator = animator;
        id = Animator.StringToHash(name);
    }
}

class AnimBoolParam : AnimParam
{
    private bool boolValue;
    public bool BoolValue
    {
        get => boolValue;
        set
        {
            boolValue = value;
            animator.SetBool(id, value);
        }
    }

    public AnimBoolParam(Animator animator, string name) : base(animator, name) { }
}
class AnimIntParam : AnimParam
{
    private int intValue;
    public int IntValue
    {
        get => intValue;
        set
        {
            intValue = value;
            animator.SetInteger(id, value);
        }
    }

    public AnimIntParam(Animator animator, string name) : base(animator, name) { }
}
class AnimFloatParam : AnimParam
{
    private float floatValue;
    public float FloatValue
    {
        get => floatValue;
        set
        {
            floatValue = value;
            animator.SetFloat(id, value);
        }
    }

    public AnimFloatParam(Animator animator, string name) : base(animator, name) { }
}
class AnimTriggerParam : AnimParam
{
    public void SetTrigger()
    {
        animator.SetTrigger(id);
    }

    public AnimTriggerParam(Animator animator, string name) : base(animator, name) { }
}

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    public Animator PlayerAnimator { get; private set; }

    private AnimTriggerParam WalkTrigger { get; set; }
    private AnimTriggerParam RunTrigger { get; set; }
    private AnimTriggerParam JumpTrigger { get; set; }

    private void Awake()
    {
        PlayerAnimator = GetComponent<Animator>();

        JumpTrigger = new AnimTriggerParam(PlayerAnimator, "JumpTrigger");
        WalkTrigger = new AnimTriggerParam(PlayerAnimator, "WalkTrigger");
        RunTrigger = new AnimTriggerParam(PlayerAnimator, "RunTrigger");
    }

    private void Start()
    {
        StartCoroutine(WaitGameStart());
    }

    public void SetJumpTrigger()
    {
        JumpTrigger.SetTrigger();
    }

    private IEnumerator WaitGameStart()
    {
        // 게임 시작 전까지 대기
        while (GameManager.Instance.IsGameStarted == false)
        {
            yield return null;
        }

        // 게임 시작 시점에 걷기 애니메이션 재생
        WalkTrigger.SetTrigger();

        yield break;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.Instance.IsGameStarted == false ||
            GameManager.Instance.IsGameOver == true)
            return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            switch (GameManager.Instance.GamePhase)
            {
                case GamePhases.SlowPhase:
                    WalkTrigger.SetTrigger();
                    break;
                case GamePhases.FastPhase:
                    RunTrigger.SetTrigger();
                    break;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (GameManager.Instance.IsGameStarted == false ||
            GameManager.Instance.IsGameOver == true)
            return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            // 점프 애니메이션 재생
            JumpTrigger.SetTrigger();
        }
    }
}