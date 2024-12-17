using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerControler : MonoBehaviour
{
    [Header("Camera")]
    public float maxSenstivity = 5000;
    public float minSenstivity = 500;
    public Slider sensitivitySlider;
    private float sensX = 3000;
    private float sensY = 3000;
    
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
    
    bool canMove = true;
    
    void Awake()
    {
        if (InputMenager.inputMenager == null)
        {
            Debug.LogError("InputMenager not found");
        }

        InputMenager.inputMenager.movmentInput += PlayerMovement;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Get character controller
        cc = GetComponent<CharacterController>();
        OnExitDialog(null);
        canMove = true;

        EventManager.Player.OnPlayerEnterDialogue += OnEnterDialog;
        EventManager.Player.OnPlayerExitDialogue += OnExitDialog;
        
        EventManager.Player.OnPlayerEnterDialogue += DisableMovement;
        EventManager.Player.OnPlayerExitDialogue += EnableMovement;
        
        // Set initial slider value based on starting sensitivity
        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = (sensX - minSenstivity) / (maxSenstivity - minSenstivity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
    
        // Reset gravity
        if (isGrounded)
        {
            velocity.y = -2f;
        }
        
        PlayerRotation();
    }

    private void OnEnterDialog(NPCConversation c, Transform t)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        canMove = false;
    }

    private void OnExitDialog(NPCConversation c)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canMove = true;
    }
    
    public void ChangeMouseSensitivity()
    {
        if(sensitivitySlider == null)
        {
            Debug.LogError("Sensitivity slider not found");
            return;
        }
        float amount = sensitivitySlider.value; // Get the value from the slider
        sensX = Mathf.Lerp(minSenstivity, maxSenstivity, amount);
        sensY = Mathf.Lerp(minSenstivity, maxSenstivity, amount);
    }

    // Player camera rotation
    private void PlayerRotation()
    {
        if(!canMove)
        {
            return;
        }
        
        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
        
        // Calculate rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        yRotation += mouseX;
        
        // Rotate Player and Camera
        transform.localRotation = Quaternion.Euler(0, yRotation, 0);
        cameraOrientation.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
    
    // Player movement
    private void PlayerMovement(Vector2 input)
    {
        if(!canMove)
        {
            return;
        }
        // Get player input
        float horizontalInput = input.x;
        float verticalInput = input.y;
        
        // Calculate movement direction
        Vector3 moveDirection = (horizontalInput * cameraOrientation.right + verticalInput * transform.forward).normalized;
        
        // Move player
        cc.Move(moveDirection * moveSpeed * Time.deltaTime);
        
        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        
        // Move player
        cc.Move(velocity * Time.deltaTime);
    }
    
    // Enable player movement
    public void EnableMovement(NPCConversation c)
    {
        canMove = true;
    }
    
    // Disable player movement
    public void DisableMovement(NPCConversation c, Transform t)
    {
        canMove = false;
    }
}
