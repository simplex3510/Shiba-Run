using Manager;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private PlayerInput playerInput;

    private Vector2 initialPosition;

    private void Awake()
    {
        initialPosition = transform.position;

        // 자동으로 Rigidbody2D 참조 연결
        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        playerInput = GetComponent<PlayerInput>();

        GameManager.Instance.OnInitializeGame += Initialize;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameStarted)
        {
            playerInput.enabled = true;
            enabled = false;
        }
    }

    public void Initialize()
    {
        transform.position = initialPosition;
        gameObject.SetActive(true);
    }
}
