using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;

    [SerializeField] private float speed = 10;
    [SerializeField] private float rotationSpeed = 180;
    [SerializeField] private float jumpHeight = 180;
    [SerializeField] private float dashDistance = 5;
    [SerializeField] private float dashSpeed = 100;

    [SerializeField] private float gravityValue = 20f;
    [SerializeField] private Vector3 friction = new Vector3(10, 10, 10);

    private float jump;

    public bool isGrounded;

    private Vector3 moveDirection = Vector3.zero;

    [SerializeField] LayerMask groundLayer;

    public void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Update()
    {
        CheckIsGrounded();
        Movement();
    }

    void CheckIsGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 1.5f, groundLayer);
    }
    void Movement()
    {
        Rotation();
        Jump();
        ForwardMovement();
        Dash();

        moveDirection = new Vector3(0, jump, speed * Input.GetAxisRaw("Vertical"));
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        Debug.Log("Dash");
        float startTime = Time.time; // need to remember this to know how long to dash
        while (Time.time < startTime + dashDistance)
        {
            controller.Move(transform.forward * dashSpeed * Time.deltaTime);
            yield return null; // this will make Unity stop here and continue next frame
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jump = jumpHeight;
        }
        if (!isGrounded)
        {
            jump -= gravityValue * Time.deltaTime;
        }
    }
    void ForwardMovement()
    {
        moveDirection = transform.TransformDirection(moveDirection); //Pedir a sergio que explique esto
        controller.Move(moveDirection * Time.deltaTime * speed);
    }
    void Rotation()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * horizontalInput * rotationSpeed * Time.deltaTime);
    }
}

