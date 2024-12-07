using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerControler))]
public class PlayerInteractionControler : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    private bool canInteract = true;
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
        
        EventManager.Player.OnPlayerEnterDialogue += DisableInteraction;
        EventManager.Player.OnPlayerExitDialogue += EnableInteraction;
        
        InputMenager.inputMenager.interationAction += Interact;
    }

    void Interact()
    {
        if(!canInteract)
        {
            return;
        }
        
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

    // Coroutine to check for interaction
    IEnumerator CheckInteraction()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(0.1f); // 10 time pre second
            
            // Raycast for detecting interactable objects
            RaycastHit hit;
            // Check if raycast hit something
            if (!Physics.Raycast(camera.position, camera.forward, out hit, 3))
            {
                interactText.text = "";
                continue;
            }
            
            // Get interactable component from hit object
            currentInteractable = hit.collider.GetComponent<Interactable>();
            if (currentInteractable != null)
            {
                interactText.text = currentInteractable.GetDescription();
            }
            else
            {
                interactText.text = "";
            }
        }
        
    }
    
    // Disable interaction
    public void DisableInteraction(NPCConversation c, Transform t)
    {
        canInteract = false;
    }
    
    // Enable interaction
    public void EnableInteraction(NPCConversation c)
    {
        canInteract = true;
    }
    
}
