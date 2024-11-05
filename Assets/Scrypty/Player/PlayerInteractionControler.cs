using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionControler : MonoBehaviour
{
    // Camera reference
    private new Transform camera;
    // Current interactable object
    private Interactable currentInteractable;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get camera orientation
        camera = GetComponent<PlayerControler>().cameraOrientation;
        
        // Start checking for interaction
        StartCoroutine(CheckInteraction());
    }
    
    // Update is called once per frame  
    void Update()
    {
        // Check if player pressed E
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Check if current interactable is not null
            if (currentInteractable == null)
            {
                return;
            }
            
            // Check if object is interactable
            if(currentInteractable.isInteractable)
            {
                currentInteractable.Interact();
            }
        }
    }

    // Coroutine to check for interaction
    IEnumerator CheckInteraction()
    {
        while (enabled)
        {
            // Debug.Log("Checking for interaction");
            // Raycast for detecting interactable objects
            RaycastHit hit;
            // Check if raycast hit something
            if (Physics.Raycast(camera.position, camera.forward, out hit, 3))
            {
                // Get interactable component from hit object
                currentInteractable = hit.collider.GetComponent<Interactable>();
            }
            yield return new WaitForSeconds(0.1f); // 10 time pre second
        }
        
    }
}
