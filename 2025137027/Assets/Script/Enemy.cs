using Unity.VectorGraphics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 50f;
    public float detectionRange = 8f;
    public float dashSpeed = 15f;
    public float stopDistance = 1.2f;
    public bool isDash = false;
    private Rigidbody rb;
    float viewAngle = 60f;
    public float viewDistance = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDash)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < detectionRange)
            {
                Vector3 toPlayer = player.position - transform.position;
                Vector3 forward = player.forward;

                float dot = Vector3.Dot(forward, toPlayer);
                float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
                if (angle < viewAngle / 2)
                {
                    Debug.Log("돌진모드");
                    isDash = true;
                }
            }
        }
        else
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance > stopDistance)
            {
                Vector3 direct = (player.position - transform.position).normalized;
                rb.MovePosition(transform.position + direct * dashSpeed * Time.deltaTime);
            }
            else
            {
                CheckParry();
                
            }


        }

        
    }


    void CheckParry()
    {
        PlayerRotate pr = player.gameObject.GetComponent<PlayerRotate>();

        if (pr.isLeftParrying || pr.isRightParrying)
        {
            Debug.Log("패링 성공");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("실패");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 forward = transform.forward * viewDistance;

        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;


        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, forward);
    }
}
