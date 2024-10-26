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
    public float gravity = -9.81f;

    public Transform cameraOrientation;
    
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    bool isGrounded;
    
    float xRotation;
    float yRotation;
    
    Vector3 velocity;
    
    CharacterController cc;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cc = GetComponent<CharacterController>();
        //rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);

        if (isGrounded)
        {
            velocity.y = -2f;
        }
        
        PlayerRotation();
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
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        // Calculate movement direction
        Vector3 moveDirection = (horizontalInput * cameraOrientation.right + verticalInput * transform.forward).normalized;
        
        // Move player
        cc.Move(moveDirection * moveSpeed * Time.deltaTime);
        
        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        
        // Move player
        cc.Move(velocity * Time.deltaTime);
    }
}
