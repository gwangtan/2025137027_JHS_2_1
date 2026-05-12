using UnityEngine;

public class BezierProjectile : MonoBehaviour
{
    private Vector3 startPoint;
    private Vector3 controlPoint1;
    private Vector3 controlPoint2;
    private Vector3 controlPoint3;
    private Vector3 controlPoint4;
    private Vector3 controlPoint5;
    private Vector3 endPoint;

    private float duration = 1.0f;
    private float elapsedTime = 0f;

    public float curveScale = 0.5f;
    private TrailRenderer trail;

    void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    public void Initialize(Vector3 targetPos, float speed)
    {
        if (trail != null) trail.Clear();

        startPoint = transform.position;
        endPoint = targetPos;

        float distance = Vector3.Distance(startPoint, endPoint);
        duration = distance / speed;

        // 시작점부터 끝점까지의 방향 벡터와 진행 거리 세분화 (1/6 등분씩)
        Vector3 dir = (endPoint - startPoint).normalized;
        float segmentDist = distance / 6f;

        // 거리에 비례한 무작위 오프셋 범위 계산
        float maxOffset = distance * curveScale;

        // 5개의 제어점을 진행 방향 축 기준 위/아래/좌/우로 랜덤 배치하여 구불구불한 유도선 형성
        controlPoint1 = startPoint + (dir * segmentDist) + GetRandomOffset(maxOffset);
        controlPoint2 = startPoint + (dir * segmentDist * 2) + GetRandomOffset(maxOffset);
        controlPoint3 = startPoint + (dir * segmentDist * 3) + GetRandomOffset(maxOffset);
        controlPoint4 = startPoint + (dir * segmentDist * 4) + GetRandomOffset(maxOffset);
        controlPoint5 = startPoint + (dir * segmentDist * 5) + GetRandomOffset(maxOffset);
    }

    // 제어점에 변형을 주기 위한 무작위 3차원 방향 벡터 생성기
    private Vector3 GetRandomOffset(float maxRange)
    {
        return new Vector3(
            Random.Range(-maxRange, maxRange),
            Random.Range(maxRange * 0.2f, maxRange), // 꼬이지 않도록 고도는 기본적으로 양수 중심 세팅
            Random.Range(-maxRange, maxRange)
        );
    }

    void Update()
    {
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // 6차 베지어 곡선 연산 구문
            float u = 1f - t;
            float u2 = u * u; float u3 = u2 * u; float u4 = u3 * u; float u5 = u4 * u; float u6 = u5 * u;
            float t2 = t * t; float t3 = t2 * t; float t4 = t3 * t; float t5 = t4 * t; float t6 = t5 * t;

            Vector3 position =
                u6 * startPoint +
                6f * u5 * t * controlPoint1 +
                15f * u4 * t2 * controlPoint2 +
                20f * u3 * t3 * controlPoint3 +
                15f * u2 * t4 * controlPoint4 +
                6f * u * t5 * controlPoint5 +
                t6 * endPoint;

            Vector3 moveDirection = position - transform.position;
            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
            }

            transform.position = position;
        }
        else
        {
            if (trail != null)
            {
                trail.transform.parent = null;
                trail.autodestruct = true;
            }
            Destroy(gameObject);
        }
    }
}
