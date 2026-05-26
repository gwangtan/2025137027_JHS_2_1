using UnityEngine;
// 1. 네임스페이스 추가
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Vector2 moveInput; // 입력 값을 저장할 변수

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // 2. 새 인풋 시스템의 메시지 시스템 감지 함수
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        // 3. Vector2 입력을 3D 공간 이동 방향(X, Z)으로 변환
        Vector3 movement = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;

        Vector3 targetPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);
    }
}
