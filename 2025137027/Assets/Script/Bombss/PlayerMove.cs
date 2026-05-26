using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private Vector2 inputDirection = Vector2.zero;
    private Vector3 velocity = Vector3.zero;

    // --- 폭발 처리를 위한 추가 변수 ---
    private Vector3 impactVelocity = Vector3.zero;
    [SerializeField] private float mass = 3f; // 플레이어의 무게 (낮을수록 멀리 날아감)
    [SerializeField] private float damping = 5f; // 감쇄 속도 (높을수록 빨리 멈춤)

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MovePlayer();
    }

    public void OnMove(InputValue value)
    {
        inputDirection = value.Get<Vector2>();
    }

    private void MovePlayer()
    {
        // 1. 일반 키보드 이동
        Vector3 moveDir = new Vector3(inputDirection.x, 0f, inputDirection.y);
        controller.Move(moveDir * moveSpeed * Time.deltaTime);

        // 2. 외부 폭발 충격(Impact) 계산 및 적용
        if (impactVelocity.magnitude > 0.2f)
        {
            controller.Move(impactVelocity * Time.deltaTime);
        }
        // 충격량을 서서히 줄여서 멈추게 함 (Lerp 효과)
        impactVelocity = Vector3.Lerp(impactVelocity, Vector3.zero, damping * Time.deltaTime);

        // 3. 중력 계산
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // --- 지뢰가 호출할 외부 충격 주입 메서드 ---
    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // 바닥으로 처박히지 않고 위로 뜨게 보정

        // F = ma -> a = F/m (힘을 질량으로 나눠 가속도를 구함)
        impactVelocity += dir * force / mass;
    }
}