using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMineSpawner : MonoBehaviour
{
    [Header("Mine Spawn Settings")]
    [SerializeField] private GameObject minePrefab;   // 소환할 지뢰 프리팹
    [SerializeField] private float spawnDistance = 3f; // 플레이어 정면으로부터 떨어진 거리

    // Player Input 컴포넌트에서 매핑한 액션 이름이 "DropMine"일 때 호출되는 함수
    public void OnDropMine(InputValue value)
    {
        if (value.isPressed && minePrefab != null)
        {
            SpawnMineInFront();
        }
    }

    private void SpawnMineInFront()
    {
        // 1. 플레이어가 바라보는 방향(정면) 계산
        Vector3 spawnDirection = transform.forward;

        // 2. 플레이어 위치에서 정면으로 spawnDistance만큼 떨어진 최종 소환 위치 계산
        Vector3 spawnPosition = transform.position + (spawnDirection * spawnDistance);

        // 지뢰가 공중에 뜨지 않도록 Y축 높이는 플레이어 발바닥 높이(또는 지면)로 맞춰줍니다.
        spawnPosition.y = transform.position.y;

        // 3. 지뢰 소환
        Instantiate(minePrefab, spawnPosition, Quaternion.identity);
    }
}