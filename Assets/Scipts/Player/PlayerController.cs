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

    private bool jumpButtonReleased = false;

    private bool isGrounded = true;
    private bool isCharging = false;
    private float holdTime = 0f;

    private float lastGroundedTime = float.MinValue;     // 지면에 있었던 마지막 시간
    private float lastJumpPressedTime = float.MinValue;  // 점프 버튼을 눌렀던 마지막 시간

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
            // 1) 점프 버퍼링
            if (Time.time - lastJumpPressedTime <= player.jumpSettings.jumpBufferTime &&
                isGrounded)
            {
                ExecuteJump();
                return;
            }

            // 2) 땅 위에서 점프
            if (isGrounded)
            {
                ExecuteJump();
            }
            // 2) 코요테 타임 점프
            else if (Time.time - lastGroundedTime <= player.jumpSettings.coyoteTime)
            {
                ExecuteJump();
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.Instance.IsGameStarted == false ||
            GameManager.Instance.IsGameOver == true)
            return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
