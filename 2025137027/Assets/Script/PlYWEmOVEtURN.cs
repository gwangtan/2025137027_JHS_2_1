using UnityEngine;

public class EnemyDot : MonoBehaviour
{
    public Transform player;
    float viewAngle = 60f;

    private void Update()
    {
        Vector3 toPlayer = player.position - transform.position;
        Vector3 forward = transform.forward;

        forward.Normalize();
        toPlayer.Normalize();

        float dot = Vector3.Dot(forward, toPlayer);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (angle < viewAngle / 2 && toPlayer.magnitude < 4f)
        {
            transform.localScale = Vector3.one * 2f;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }
}