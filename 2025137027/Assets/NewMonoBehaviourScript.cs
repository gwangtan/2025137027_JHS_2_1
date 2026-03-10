using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class ClickToMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 mouseScreenPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isSprinting;

    public void OnPoint(InputValue value)
    {
        mouseScreenPosition = value.Get<Vector2>(); // 마우스 위치 업데이트
    }

    public void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
            RaycastHit[] hits = Physics.RaycastAll(ray); // 레이저 경로에 있는 모든 물체를 탐색

            foreach (RaycastHit hit in hits) // 모든 물체에 한해 반복
            {
                if (hit.collider.gameObject != gameObject) // 부딪힌 물체가 나 자신이 아닐 때만
                {
                    targetPosition = hit.point; // Plane에 부딪힌 지점을 타겟
                    targetPosition.y = transform.position.y;
                    isMoving = true;

                    break; // 탐색 했으니 foreach 반복 중단
                }
            }
        }
    }

    void Update()
    {
        if (isMoving)
        {
            Vector3  direction = targetPosition - transform.position;
            float magnitude = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y + direction.z * direction.z);
            //Vector3 normalizeVector = new Vector3(direction.x / magnitude, direction.y / magnitude, direction.z / magnitude);
            Vector3 normalizeVector = direction / magnitude;

             float speed = moveSpeed;

            if ( isSprinting )
            {
                speed *= 2;
                Debug.Log("1");
            }

            transform.Translate(normalizeVector * speed * Time.deltaTime);

            float x = Mathf.Pow(targetPosition.x - transform.position.x,2);
            float y = Mathf.Pow(targetPosition.y - transform.position.y,2);
            float z = Mathf.Pow(targetPosition.z - transform.position.z,2);


            if (Mathf.Sqrt(x + y + z) < 0.1)
            {
                isMoving = false;
            }

           
        }

    }
    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed; // 버튼을 누르고 있으면 true, 떼면 false
    }



}