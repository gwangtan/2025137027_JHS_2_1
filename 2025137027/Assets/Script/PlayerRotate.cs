using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotate : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotationSpeed = 100f;
    private Vector2 moveInput;
    public bool isLeftParrying = false;
    public bool isRightParrying = false;
    
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLeftParry(InputValue value)
    {
        isLeftParrying = value.isPressed;
    }

    public void OnRightParry(InputValue value)
    {
        isRightParrying = value.isPressed;
    }

    // Update is called once per frame
    void Update()
    {
      //float rotation = moveInput.x * rotationSpeed *Time.deltaTime;
        //transform.Rotate(0f, rotation, 0f);

        float rotation = moveInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotation, 0f);



        Vector3 moveDir = moveInput.y * moveSpeed * Time.deltaTime * transform.forward;
        transform.position += moveDir;
        
    }
}
