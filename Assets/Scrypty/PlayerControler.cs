using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [Header("Camera")]
    public float sensX;
    public float sensY;
    
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;

    public Transform cameraOrientation;
    
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    bool isGrounded;
    
    float xRotation;
    float yRotation;
    
    float horizontalInput;
    float verticalInput;
    
    Vector3 moveDirection;
    
    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
        
        PlayerRotation();
        SpeedControl();
        
        if(isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
    
    void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerRotation()
    {
        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        yRotation += mouseX;
        
        // Rotate Player and Camera
        transform.localRotation = Quaternion.Euler(0, yRotation, 0);
        cameraOrientation.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
    
    private void PlayerMovement()
    {
        // Get player input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        // Calculate movement direction
        moveDirection = (horizontalInput * cameraOrientation.right + verticalInput * transform.forward).normalized;
        
        // Move player
        rb.AddForce(moveDirection * moveSpeed * 10f);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        
        if(flatVel.magnitude > moveSpeed)
        {
            flatVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(flatVel.x, rb.velocity.y, flatVel.z);
        }
    }
}
