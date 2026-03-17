using UnityEngine;

public class Playernodot : MonoBehaviour
{
    public Transform player;
    public float viewAngle = 60f; // 시야각

    float DotProject(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    void Update()
    {
        Vector3 a = (player.position - transform.position).normalized;
        Vector3 b = transform.forward;

            float dot = DotProject(a, b);
        float angle = Mathf.Acos(dot) * Mathf.Deg2Rad;


        

        if (angle < viewAngle / 2)
        {
            Debug.Log("플레이어가 시야 안에 있음!");
        }
    }
}
