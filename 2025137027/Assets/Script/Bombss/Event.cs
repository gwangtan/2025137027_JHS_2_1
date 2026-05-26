using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EventDirector : MonoBehaviour
{
    [Header("Spawn Target")]
    [SerializeField] private GameObject fallingObjectPrefab; // 하강할 오브젝트 프리팹
    private Vector3 spawnPosition = new Vector3(-14f, 390f, -9f); // 지정된 좌표

    [Header("Camera Settings")]
    [SerializeField] private Camera mainCamera;       // 원래 메인 카메라
    [SerializeField] private Camera eventCamera;      // 연출용 지정 카메라
    [SerializeField] private float cameraCutDuration = 4f; // 시점 전환 시간 (4초)

    [Header("Shake Settings")]
    [SerializeField] private float shakeMagnitude = 0.2f; // 진동 세기 (좌우 움직임 폭)
    [SerializeField] private float shakeSpeed = 50f;      // 진동 속도 (얼마나 빠르게 왔다갔다할지)

    private void Start()
    {
        // 시작할 때 메인 카메라는 켜고, 이벤트 카메라는 꺼둡니다.
        if (mainCamera != null) mainCamera.gameObject.SetActive(true);
        if (eventCamera != null) eventCamera.gameObject.SetActive(false);
    }

    // Player Input 컴포넌트에서 매핑한 액션 이름이 "TriggerEvent"일 때 호출되는 함수
    public void OnTriggerEvent(InputValue value)
    {
        if (value.isPressed && fallingObjectPrefab != null)
        {
            // 이미 이벤트 카메라가 켜져 있는 동안 중복 실행 방지
            if (eventCamera != null && eventCamera.gameObject.activeSelf) return;

            // 1. 지정된 좌표에 오브젝트 생성
            Instantiate(fallingObjectPrefab, spawnPosition, Quaternion.identity);

            // 2. 카메라 시점 전환 및 진동 코루틴 시작
            StartCoroutine(CameraCutsceneRoutine());
        }
    }

    private IEnumerator CameraCutsceneRoutine()
    {
        // 안전성 체크
        if (mainCamera == null || eventCamera == null)
        {
            Debug.LogWarning("카메라가 지정되지 않았습니다.");
            yield break;
        }

        // 두 카메라의 원래 시작 위치 저장 (진동 후 복구를 위해)
        Vector3 originalMainCamPos = mainCamera.transform.localPosition;
        Vector3 originalEventCamPos = eventCamera.transform.localPosition;

        // 연출용 지정 카메라로 시점 전환
        mainCamera.gameObject.SetActive(false);
        eventCamera.gameObject.SetActive(true);

        float elapsedTime = 0f;

        // 지정된 시간(4초) 동안 루프를 돌며 두 카메라를 모두 흔듭니다.
        while (elapsedTime < cameraCutDuration)
        {
            elapsedTime += Time.deltaTime;

            // 좌우(X축)로 빠르게 진동하는 오프셋 계산
            float shakeX = Mathf.Sin(Time.time * shakeSpeed) * shakeMagnitude;

            // 1. 이벤트 카메라 흔들기 (현재 화면에 보이는 카메라)
            eventCamera.transform.localPosition = new Vector3(originalEventCamPos.x + shakeX, originalEventCamPos.y, originalEventCamPos.z);

            // 2. 메인 카메라 흔들기 (비활성화 상태여도 위치를 미리 흔들어둠)
            mainCamera.transform.localPosition = new Vector3(originalMainCamPos.x + shakeX, originalMainCamPos.y, originalMainCamPos.z);

            // 다음 프레임까지 대기
            yield return null;
        }

        // 진동이 끝난 후 두 카메라 위치를 모두 원래대로 정밀하게 복구
        eventCamera.transform.localPosition = originalEventCamPos;
        mainCamera.transform.localPosition = originalMainCamPos;

        // 다시 원래 메인 카메라로 복귀
        eventCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
    }
}