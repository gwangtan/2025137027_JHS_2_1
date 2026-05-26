using System.Collections; // 코루틴 사용을 위해 추가
using UnityEngine;

public class FallingBomb : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float fallSpeed = 3f; // 내려오는 속도 (천천히)

    [Header("Explosion Settings")]
    [SerializeField] private float radius = 10f;
    [SerializeField] private float force = 2000f;
    [SerializeField] private float upwardsModifier = 2f;

    [Header("New: Scale Up Visual Settings")]
    [SerializeField] private GameObject visualPrefab; // 1000배로 커질 이펙트/오브젝트 프리팹 등록
    [SerializeField] private float targetScaleMultiplier = 1000f; // 목표 배율 (1000배)
    [SerializeField] private float growthDuration = 2f;          // 커지는 시간 (2초)
    [SerializeField] private float lifeTimeAfterGrowth = 2f;      // 유지 시간 (2초)

    private bool isExploded = false;

    private void Update()
    {
        // 매 프레임 지정된 속도로 천천히 하강
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 어느 오브젝트(트리거가 아닌 실제 물리 표면)와든 부딪히면 폭발
        Explode();
    }

    private void Explode()
    {
        if (isExploded) return;
        isExploded = true;

        Vector3 explosionPos = transform.position;

        // --- [추가] 지정된 프리팹 생성 및 1000배 확대 코루틴 실행 ---
        if (visualPrefab != null)
        {
            // 이 폭탄 오브젝트 자체는 마지막에 Destroy(gameObject)로 사라지므로,
            // 별도의 독립된 GameObject를 생성하여 스케일 업 코루틴을 넘겨줍니다.
            GameObject spawnedVisual = Instantiate(visualPrefab, explosionPos, Quaternion.identity);
            StartCoroutine(ScaleUpRoutine(spawnedVisual));
        }
        // -------------------------------------------------------------

        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        foreach (var col in colliders)
        {
            float distance = Vector3.Distance(col.transform.position, explosionPos);
            float attenuation = 1f - Mathf.Clamp01(distance / radius);

            // 1. 플레이어에게 충격 전달 (PlayerMovement 스크립트 대응)
            PlayerMove playerMove = col.GetComponent<PlayerMove>();
            if (playerMove != null)
            {
                Vector3 dir = col.transform.position - explosionPos;
                dir.y += upwardsModifier;
                playerMove.AddImpact(dir, force * attenuation);
                continue;
            }

            // 2. 일반 Rigidbody 오브젝트 대응
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

        // 메쉬와 콜라이더를 숨겨서 폭탄 본체는 즉시 사라진 것처럼 보이게 처리
        // (코루틴이 이 스크립트에서 돌고 있기 때문에 즉시 Destroy하면 코루틴이 멈추는 것을 방지)
        DisableBombComponents();

        // 총 연출 시간(2초 확대 + 2초 대기 = 4초) 뒤에 폭탄 본체 오브젝트 완전히 제거
        Destroy(gameObject, growthDuration + lifeTimeAfterGrowth);
    }

    // 생성된 이펙트 프리팹을 2초 동안 1000배로 키우고 2초 뒤 삭제하는 코루틴
    private IEnumerator ScaleUpRoutine(GameObject targetObj)
    {
        if (targetObj == null) yield break;

        Vector3 originalScale = targetObj.transform.localScale;
        Vector3 targetScale = originalScale * targetScaleMultiplier;
        float elapsedTime = 0f;

        // 1. 2초 동안 크기를 1000배로 부드럽게 확대
        while (elapsedTime < growthDuration)
        {
            if (targetObj == null) yield break; // 혹시 모를 예외 처리

            elapsedTime += Time.deltaTime;
            targetObj.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / growthDuration);
            yield return null;
        }

        if (targetObj != null) targetObj.transform.localScale = targetScale;

        // 2. 다 커진 상태에서 2초 동안 대기
        yield return new WaitForSeconds(lifeTimeAfterGrowth);

        // 3. 생성했던 프리팹 오브젝트 삭제
        if (targetObj != null)
        {
            Destroy(targetObj);
        }
    }

    // 폭탄이 충돌하는 순간 겉모습만 바로 지워주는 편의용 함수
    private void DisableBombComponents()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null) renderer.enabled = false;

        var collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        // 자식 오브젝트가 있다면 자식들도 다 숨김 처리
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}