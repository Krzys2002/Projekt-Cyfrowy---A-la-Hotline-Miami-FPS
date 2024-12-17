using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool isInteractable = true;
    
    public UnityEvent<GameObject> OnInteract;
    
    public string Description;
    
    public AudioClip sound;
    
    private AudioSource audioSource;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null)
        {
            Debug.LogError("Audio source not found on object: " + gameObject.name);
        }
    }
    
    public void SetInteractable(bool value)
    {
        isInteractable = value;
    }
    
    public void Interact()
    {
        // check if object is interactable
        if (!isInteractable)
        {
            return;
        }
        
        // invoke on interact event
        OnInteract.Invoke(gameObject);
        if(sound != null && audioSource != null)
        {
            audioSource.PlayOneShot(sound);
        }
        
        // check if someone is listening to the event
        if (EventManager.Objects.OnObjectInteract != null)
        {
            // invoke on object interact event
            EventManager.Objects.OnObjectInteract.Invoke(gameObject, 0);
        }
    }

    public string GetDescription()
    {
        return Description;
    }
}
