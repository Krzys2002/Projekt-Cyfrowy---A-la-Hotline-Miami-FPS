using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GateColider : MonoBehaviour
{
    // Player in gate
    public bool playerIn = false;
    
    // Events
    public UnityEvent onPlayerEnter;
    public UnityEvent onPlayerExit;
    
    // on trigger enter is called when collider enters gate
    private void OnTriggerEnter(Collider other)
    {
        // check if collider is player
        if(other.tag == "Player")
        {
            // set player in gate
            playerIn = true;
            // invoke on player enter event
            onPlayerEnter.Invoke();
        }
    }
    
    // on trigger exit is called when collider exits gate
    private void OnTriggerExit(Collider other)
    {
        // check if collider is player
        if(other.tag == "Player")
        {
            // set player out of gate
            playerIn = false;
            // invoke on player exit event
            onPlayerExit.Invoke();
        }
    }
}
