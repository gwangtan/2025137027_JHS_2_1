using UnityEngine;

public class Teiyang : MonoBehaviour
{
    void Update()
    {
        float speed = 20f;
        float angle = 360f; // 이동할 방향 (도 단위)
        float radians = angle * Mathf.Deg2Rad;

        Vector3 direction = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians));
        transform.position += direction * speed * Time.deltaTime;
    }
}
