using UnityEngine;

[System.Serializable]
public class JumpSettings
{
    public float minForce = 5f;     // 최소 점프력
    public float maxForce = 10f;    // 최대 점프력
    public float maxHoldTime = 0.5f; // 힘이 최대치에 도달하는 시간 (초)

    public float coyoteTime = 0.1f;    // 착지 직후 점프 허용 시간
    public float jumpBufferTime = 0.1f;// 점프 입력 버퍼 시간
}

public class Player : MonoBehaviour
{
    [Header("Jump Settings")]
    public JumpSettings jumpSettings = new JumpSettings();

    [Header("References")]
    public Rigidbody2D rb;

    private void Awake()
    {
        // 자동으로 Rigidbody2D 참조 연결
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.linearVelocity = new Vector2(0.0f, rb.linearVelocityY);
        }
    }
}
