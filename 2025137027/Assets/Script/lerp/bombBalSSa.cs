using UnityEngine;
using UnityEngine.InputSystem;

public class bombBalSSa : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public Transform targetEnemy; // 타겟 적 오브젝트 지정
    public float projectileSpeed = 10f;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.aKey.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (targetEnemy == null)
        {
            Debug.LogWarning("적(Target Enemy)이 지정되지 않았습니다.");
            return;
        }

        for (int i = 0; i < 12; i++)
        {
            GameObject projObj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            BezierProjectile projectile = projObj.GetComponent<BezierProjectile>();

            if (projectile != null)
            {
                // 적 오브젝트의 현재 위치를 최종 목적지로 전달
                projectile.Initialize(targetEnemy.position, projectileSpeed);
            }
        }
    }
}
