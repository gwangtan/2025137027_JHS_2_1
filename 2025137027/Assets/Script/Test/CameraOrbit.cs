using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [Header("Target Tracking")]
    public Transform target;

    // [수정] MouseRaycastTest에서 넘겨받을 2D 입력 벡터 (WASD)
    [Header("Movement Input")]
    public Vector2 moveInput = Vector2.zero;

    [Header("Orbit Settings")]
    public float rotateSpeed = 100f;
    public float distance = 8f; // 공과의 기본 유지 거리

    [Header("Angle Limits")]
    [Tooltip("카메라가 바닥 밑으로 내려가지 못하게 막는 최소 각도입니다.")]
    public float minPitch = 10f;
    [Tooltip("카메라가 머리 꼭대기에서 뒤집히지 않게 막는 최대 각도입니다.")]
    public float maxPitch = 80f;

    private float yaw = 0f;   // 좌우 회전각 (A/D)
    private float pitch = 30f; // 상하 회전각 (W/S) - 초기 각도 30도

    void Update()
    {
        // 턴에 맞는 타겟 공 자동 추적
        if (TurnGameManager.Instance != null)
        {
            if (TurnGameManager.Instance.currentTurn == TurnGameManager.Turn.Player1)
            {
                if (TurnGameManager.Instance.player1Ball != null)
                    target = TurnGameManager.Instance.player1Ball.transform;
            }
            else
            {
                if (TurnGameManager.Instance.player2Ball != null)
                    target = TurnGameManager.Instance.player2Ball.transform;
            }
        }

        if (target == null) return;

        // [수정] WASD 입력에 따른 각도 변화 연산
        // A, D 누르면 좌우 회전(yaw)
        yaw += moveInput.x * rotateSpeed * Time.deltaTime;
        
        // W, S 누르면 상하 회전(pitch). 
        // 팁: 보통 W를 누를 때 카메라가 올라가 내려다보는 게 자연스러우므로 더해줍니다.
        pitch += moveInput.y * rotateSpeed * Time.deltaTime;

        // 카메라가 너무 숙여지거나 뒤집히지 않도록 상하 각도 제한 구속
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // [수정] 3차원 구면 좌표계를 이용한 오프셋 포지션 계산
        // 쿼터니언을 좌우(Y축), 상하(X축) 순으로 조합합니다.
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        
        // 정면 방향 벡터(Vector3.forward)에 회전값을 곱하고 거리를 주어 오프셋 위치를 구합니다.
        Vector3 rotatedOffset = rotation * new Vector3(0, 0, -distance);

        // 최종 카메라 위치 적용: 공의 중심 기준 + 회전된 오프셋 거리
        transform.position = target.position + rotatedOffset;

        // 카메라 시선은 무조건 타겟 공을 향하도록 고정
        transform.LookAt(target);
    }
}