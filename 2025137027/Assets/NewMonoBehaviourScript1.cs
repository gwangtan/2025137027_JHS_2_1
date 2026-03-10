using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private bool isSprinting;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }



    void Update()
    {
        float speed = moveSpeed;
        if (isSprinting)
        {
            speed *= 2;
            Debug.Log("1");
        }

        Vector3 direction = new Vector3(moveInput.x, moveInput.y, 0);
        transform.Translate(direction * speed * Time.deltaTime);

        

        
    }


}