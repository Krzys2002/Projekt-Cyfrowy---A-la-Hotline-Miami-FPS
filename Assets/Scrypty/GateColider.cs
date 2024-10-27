using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GateColider : MonoBehaviour
{
    public bool playerIn = false;
    
    public UnityEvent onPlayerEnter;
    public UnityEvent onPlayerExit;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerIn = true;
            onPlayerEnter.Invoke();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            playerIn = false;
            onPlayerExit.Invoke();
        }
    }
}
