using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Manager;
using NUnit.Framework;
using UnityEditor.Build;

/* Memo
* 1. 나중에 UI에서 점프력 게이지를 표시하려면 holdTime을 퍼센트로 변환하는 로직이 필요함
*/

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    private Player player;

    [SerializeField] private bool jumpButtonReleased = false;

    [SerializeField] private bool isGrounded = true;
    [SerializeField] private bool isCharging = false;
    [SerializeField] private float holdTime = 0f;

    [SerializeField] private float lastGroundedTime = float.MinValue;     // 지면에 있었던 마지막 시간
    [SerializeField] private float lastJumpPressedTime = float.MinValue;  // 점프 버튼을 눌렀던 마지막 시간

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

        if (isGrounded)
        {
            // Coyote Time 갱신
            lastGroundedTime = Time.time;
        }

        if (jumpButtonReleased)
        {
            // 1) 착지 직전 - 점프 버퍼링
            if (isGrounded && Time.time - lastJumpPressedTime < player.jumpSettings.jumpBufferTime)
            {
                ExecuteJump();
                return;
            }

            // 2) 착지 중 - 땅 위에서 점프
            if (isGrounded)
            {
                ExecuteJump();
                return;
            }

            // 3) 착지 직후 - 코요테 타임 점프
            else if (Time.time - lastGroundedTime < player.jumpSettings.coyoteTime)
            {
                ExecuteJump();
                return;
            }
        }
    }


    // PlayerInput 컴포넌트가 Jump 액션을 호출할 때 실행됨
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isCharging = true;
            holdTime = 0f;
            lastJumpPressedTime = Time.time;
        }

        if (context.canceled)
        {
            isCharging = false;

            jumpButtonReleased = true;
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

        jumpButtonReleased = false;

        SoundManager.Instance.PlaySFX(AudioClipNames.Jump);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.Instance.IsGameStarted == false ||
            GameManager.Instance.IsGameOver == true)
            return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f) // 위쪽에서 눌린 경우
                {
                    isGrounded = true;
                    return;
                }
            }
        }

        isGrounded = false;
    }
}
