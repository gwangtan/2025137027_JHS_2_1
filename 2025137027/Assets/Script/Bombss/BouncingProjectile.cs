using UnityEngine;

public class BouncingProjectile : MonoBehaviour
{
    [Header("Movement & Physics")]
    public Vector3 velocity = new Vector3(2f, 5f, 0f); // 초기 속도 (소환할 때 변경 가능)
    public Vector3 gravity = new Vector3(0, -9.81f, 0);
    [Range(0f, 1f)] public float damping = 0.7f;

    [Header("Bounce Settings")]
    public int maxBounces = 3;
    private int currentBounces = 0;

    [Header("Explosion Settings")]
    public float radius = 5f;
    public float force = 300f;
    public float upwardsModifier = 1f;

    private bool isExploded = false;

    private void Update()
    {
        // 중력 및 이동 수동 계산 (기존 ReflectAuto 방식)
        velocity += gravity * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isExploded) return;

        // 1. Enemy 태그와 충돌 시 즉시 폭발
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Explode();
            return;
        }

        // 2. 최대 바운스 횟수 체크
        if (currentBounces >= maxBounces)
        {
            Explode();
            return;
        }

        // 3. 반사각 계산 및 속도 감쇄 (기존 ReflectAuto 방식)
        Vector3 normal = collision.contacts[0].normal.normalized;
        float dot = Vector3.Dot(velocity, normal);

        // 벽을 마주보고 부딪힐 때만 반사 (파고들기 방지)
        if (dot < 0)
        {
            Vector3 reflect = velocity - 2f * dot * normal;
            velocity = reflect * damping;
            currentBounces++;
        }
    }

    private void Explode()
    {
        isExploded = true;
        Vector3 explosionPos = transform.position;

        // 폭발 범위 내의 콜라이더 검출
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (var col in colliders)
        {
            Rigidbody rb = col.attachedRigidbody;
            if (rb == null) continue;

            Vector3 toTarget = rb.position - explosionPos;
            float distance = toTarget.magnitude;
            Vector3 dir = toTarget.normalized;

            // 거리에 따른 위력 감쇄
            float attenuation = 1f - Mathf.Clamp01(distance / radius);

            // 상향 힘 추가 및 정규화
            dir += Vector3.up * upwardsModifier;
            dir = dir.normalized;

            Vector3 impulse = dir * force * attenuation;
            rb.AddForce(impulse, ForceMode.Impulse);
        }

        // 오브젝트 제거
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}