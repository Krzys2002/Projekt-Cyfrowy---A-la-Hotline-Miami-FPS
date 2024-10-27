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
        if (isInteractable)
        {
            OnInteract.Invoke(gameObject);
        }
    }
}
