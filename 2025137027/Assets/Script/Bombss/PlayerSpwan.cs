using UnityEngine;
using UnityEngine.InputSystem; // New Input System 필요

public class PlayerSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject projectilePrefab; // 생성할 오브젝트 프리팹
    [SerializeField] private Transform spawnPoint;         // 오브젝트가 발사될 위치
    [SerializeField] private float launchSpeed = 10f;      // 마우스 방향으로 날아갈 속력

    private Camera mainCamera;

    private void Start()
    {
        // 빈번한 Camera.main 호출을 피하기 위해 캐싱
        mainCamera = Camera.main;
    }

    // Player Input 컴포넌트에서 'A' 키에 매핑된 Action 이름이 "Fire"일 때 실행됩니다.
    public void OnFire(InputValue value)
    {
        if (value.isPressed && projectilePrefab != null)
        {
            SpawnProjectileTowardsMouse();
        }
    }

    private void SpawnProjectileTowardsMouse()
    {
        // 1. 발사 시작 위치 결정 (지정 안 되어 있으면 플레이어 위치)
        Vector3 startPos = spawnPoint != null ? spawnPoint.position : transform.position;

        // 2. 현재 마우스의 화면상 위치 가져오기 (New Input System 방식)
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        // 3. 화면 좌표를 게임 월드(3D) 좌표로 변환
        // 단, ScreenToWorldPoint는 카메라와의 거리(z축)가 필요하므로 현재 발사 위치와 카메라 사이의 거리를 넣어줍니다.
        float distanceFromCamera = Mathf.Abs(mainCamera.transform.position.z - startPos.z);
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, distanceFromCamera));

        // 2D 게임(Z축 고정)인 경우 마우스 좌표의 Z값을 발사 위치의 Z값과 동기화하여 수평면 상 오차 방지
        mouseWorldPos.z = startPos.z;

        // 4. 발사 위치에서 마우스 위치를 향하는 방향 벡터(Direction) 계산
        Vector3 launchDirection = (mouseWorldPos - startPos).normalized;

        // 5. 오브젝트 생성 및 초기 속도(방향 * 속력) 주입
        GameObject projGo = Instantiate(projectilePrefab, startPos, Quaternion.identity);
        BouncingProjectile projectile = projGo.GetComponent<BouncingProjectile>();

        if (projectile != null)
        {
            // 계산된 방향과 속력을 기존 스크립트의 velocity에 할당합니다.
            projectile.velocity = launchDirection * launchSpeed;
        }
    }
}