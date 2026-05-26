using UnityEngine;

public class Landmine : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float delay = 5f;
    public float radius = 7f;
    public float force = 1500f; // 플레이어가 시원하게 날아가도록 힘을 상향 조정했습니다.
    public float upwardsModifier = 1.5f;

    private bool isExploded = false;

    void Start()
    {
        Invoke("Explode", delay);
    }

    void Explode()
    {
        if (isExploded) return;
        isExploded = true;

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        foreach (var col in colliders)
        {
            float distance = Vector3.Distance(col.transform.position, explosionPos);
            // 거리에 따른 위력 감쇄 (가까울수록 강하게)
            float attenuation = 1f - Mathf.Clamp01(distance / radius);

            // 1. 플레이어 이동 스크립트가 있는지 확인 (CharacterController 대응)
            PlayerMove playerMove = col.GetComponent<PlayerMove>();
            if (playerMove != null)
            {
                Vector3 dir = col.transform.position - explosionPos;
                dir.y += upwardsModifier; // 상향 힘 추가

                // 플레이어에게 수동 충격 전달!
                playerMove.AddImpact(dir, force * attenuation);
                continue; // 플레이어 처리를 완료했으므로 다음 오브젝트로
            }

            // 2. 일반 적이나 오브젝트 (Rigidbody 대응)
            Rigidbody rb = col.attachedRigidbody;
            if (rb != null)
            {
                Vector3 toTarget = rb.position - explosionPos;
                Vector3 dir = toTarget.normalized;
                dir += Vector3.up * upwardsModifier;
                dir = dir.normalized;

                rb.AddForce(dir * force * attenuation, ForceMode.Impulse);
            }
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}