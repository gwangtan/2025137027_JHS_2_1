using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementWithTurn : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 5f;

    Vector3 moveInput;
    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveInput = new Vector3(input.x, 0, input.y);
    }

    private void Update()
    {
        if (GetMagnitudeVector3(moveInput) > 0)
        {
            //transform.Translate(GetNormalizedVector3(moveInput) * moveSpeed * Time.deltaTime);
            transform.position += (GetNormalizedVector3(moveInput) * moveSpeed * Time.deltaTime);
            LookAtDirection(moveInput);
        }
    }

    void LookAtDirection(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public Vector3 GetNormalizedVector3(Vector3 vector)
    {
        float magnitude = GetMagnitudeVector3(vector);

        if (magnitude > 0)
        {
            return vector / magnitude;
        }
        else
        {
            return Vector3.zero;
        }
    }
    float GetMagnitudeVector3(Vector3 vector) => Mathf.Sqrt((vector.x * vector.x) + (vector.y * vector.y) + (vector.z * vector.z));

}