using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionControler : MonoBehaviour
{
    
    private new Transform camera;
    private Interactable currentInteractable;
    
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<PlayerControler>().cameraOrientation;

        StartCoroutine(CheckInteraction());
    }
    
    // Update is called once per frame  
    void Update()
    {
        StartCoroutine(CheckInteraction());
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentInteractable != null)
            {
                if(currentInteractable.isInteractable)
                {
                    currentInteractable.Interact();
                }
            }
        }
    }

    IEnumerator CheckInteraction()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, 3))
        {
            if (hit.collider != null)
            {
                 currentInteractable = hit.collider.GetComponent<Interactable>();
            }
        }
        yield return new WaitForSeconds(0.1f); // 10 time pre second
    }
}
