using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRaycastTest : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float rayDistance = 100f;

    [Header("Hit Settings")]
    public float hitPower = 1000f; // 힘이 부족할 수 있어 기본값을 조금 올렸습니다.

    [Header("References")]
    public CameraOrbit cam;

    // [수정] 좌우(X), 위아래(Y) 입력을 모두 담기 위한 Vector2 변수
    private Vector2 moveInput;

    public void OnMove(InputValue value)
    {
        // WASD 입력을 Vector2(X: A/D, Y: W/S) 형태로 받아옵니다.
        moveInput = value.Get<Vector2>();

        // 카메라 스크립트에 전체 입력을 넘겨줍니다.
        if (cam != null)
        {
            cam.moveInput = moveInput;
        }
    }

    public void OnClick(InputValue value)
    {
        if (!value.isPressed) return;

        if (TurnGameManager.Instance == null) return;

        // 규칙 3: 공이 움직이는 중에는 입력 차단
        if (TurnGameManager.Instance.isBallMoving)
        {
            Debug.Log("공들이 아직 완전히 멈추지 않았습니다.");
            return;
        }

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            Rigidbody rb = hit.collider.attachedRigidbody;

            if (rb != null)
            {
                // 규칙 2: 자기 턴에 맞는 공만 치기
                TurnGameManager.Turn currentTurn = TurnGameManager.Instance.currentTurn;
                Rigidbody p1Ball = TurnGameManager.Instance.player1Ball;
                Rigidbody p2Ball = TurnGameManager.Instance.player2Ball;

                if (currentTurn == TurnGameManager.Turn.Player1 && rb != p1Ball) return;
                if (currentTurn == TurnGameManager.Turn.Player2 && rb != p2Ball) return;

                // 공 치기: 카메라가 바라보는 평면 방향으로 밀쳐내기
                Vector3 pushDirection = ray.direction;
                pushDirection.y = 0;

                rb.AddForce(pushDirection.normalized * hitPower);
                Debug.Log($"{currentTurn} 타격!");
            }
        }
    }
}