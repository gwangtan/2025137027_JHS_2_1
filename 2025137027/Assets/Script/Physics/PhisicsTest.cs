using UnityEngine;
using UnityEngine.InputSystem;

public class PhisicsTest : MonoBehaviour
{
    private Rigidbody rb;
    public float forcePower = 10f;
    [SerializeField] private float speed;
    private bool isSprint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.forward * 10f , ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
     speed = rb.linearVelocity.magnitude;   
    }

    void FixedUpdate()
    {
        if (isSprint)
        {
            rb.AddForce(Vector3.forward * forcePower, ForceMode.Force);
        }
    }

    public void OnSprint(InputValue value)
    {
        isSprint = value.isPressed;
    }
}
