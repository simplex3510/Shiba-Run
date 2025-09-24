using UnityEngine;
using UnityEngine.InputSystem;

/* Memo
* 1. 나중에 UI에서 점프력 게이지를 표시하려면 holdTime을 퍼센트로 변환하는 로직이 필요함
*/

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public bool IsGround { get { return isGrounded; } }

    private Player player;

    private bool isGrounded = true;
    private bool isCharging = false;
    private float holdTime = 0f;

    private float lastGroundedTime = 0.0f;     // 지면에 있었던 마지막 시간
    private float lastJumpPressedTime = -1.0f;  // 점프 버튼을 눌렀던 마지막 시간

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        // 누르고 있는 동안 holdTime 증가
        if (isCharging)
        {
            holdTime += Time.deltaTime;
        }

        // Coyote Time 갱신
        if (IsGrounded())
        {
            lastGroundedTime = Time.time;
        }

        // Jump Buffer 및 Coyote Time 확인
        if ((Time.time - lastJumpPressedTime <= player.jumpSettings.jumpBufferTime) &&
            (Time.time - lastGroundedTime <= player.jumpSettings.coyoteTime))
        {
            ExecuteJump();
            lastJumpPressedTime = float.MinValue; // 한번 사용 후 초기화
        }
    }

    
    // PlayerInput 컴포넌트가 Jump 액션을 호출할 때 실행됨
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // 점프 버튼 누름 시작
            isCharging = true;
            holdTime = 0f;
            lastJumpPressedTime = Time.time;
        }
        else if (context.canceled)
        {
            // 버튼을 뗐을 때
            isCharging = false;
        }
    }

    private void ExecuteJump()
    {
        // 누른 시간 비율 계산
        float t = Mathf.Clamp01(holdTime / player.jumpSettings.maxHoldTime);
        float jumpForce = Mathf.Lerp(player.jumpSettings.minForce, player.jumpSettings.maxForce, t);

        player.rb.linearVelocity = new Vector2(player.rb.linearVelocityX, jumpForce);
        isGrounded = false;
        isCharging = false;
        holdTime = 0f;
    }

    private bool IsGrounded()
    {
        // 코요테 타임 체크
        return player.rb.linearVelocityY <= 0 && isGrounded;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
