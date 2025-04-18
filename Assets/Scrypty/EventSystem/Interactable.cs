using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool isInteractable = true;
    
    public UnityEvent<GameObject> OnInteract;
    
    public void SetInteractable(bool value)
    {
        isInteractable = value;
    }
    
    public void Interact()
    {
        // check if object is interactable
        if (isInteractable)
        {
            // invoke on interact event
            OnInteract.Invoke(gameObject);
            // check if someone is listening to the event
            if (EventManager.Objects.OnObjectInteract != null)
            {
                // invoke on object interact event
                EventManager.Objects.OnObjectInteract.Invoke(gameObject, 0);
            }
        }
    }
}
