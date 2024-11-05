using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    // colliders for gate
    [SerializeField]
    private BoxCollider colliderA;
    [SerializeField]
    private BoxCollider colliderB;
    
    // sublevels for gate
    [SerializeField]
    public SubLevel subLevelA;
    [SerializeField]
    public SubLevel subLevelB;
    
    // player in gate
    bool playerInA = false;
    bool playerInB = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // register gate in sublevels
        subLevelA.addGate(this);
        subLevelB.addGate(this);
    }
    
    // called when player enters collider A
    public void playerEnterA()
    {
        playerInA = true;
    }
    
    // called when player exits collider A
    public void playerExitA()
    {
        playerInA = false;
        
        // check if player is not in B
        if(!playerInB)
        {
            // player entered sublevel A
            subLevelA.playerEnter(this);
            subLevelB.playerExit(this);
        }
    }
    
    // called when player enters collider B
    public void playerEnterB()
    {
        playerInB = true;
    }
    
    // called when player exits collider B
    public void playerExitB()
    {
        playerInB = false;
        
        // check if player is not in A
        if(!playerInA)
        {
            // player entered sublevel B
            subLevelB.playerEnter(this);
            subLevelA.playerExit(this);
        }
    }
    
    // activate close sublevels
    public void activateCloseSublevels(SubLevel subLevel)
    {
        // check which sublevel is calling
        if(subLevelA == subLevel)
        {
            subLevelB.ActivateEnemies();
        }
        else
        {
            subLevelA.ActivateEnemies();
        }
    }
    
    // deactivate close sublevels
    public void deactivateCloseSublevels(SubLevel subLevel)
    {
        // check which sublevel is calling
        if(subLevelA == subLevel)
        {
            subLevelB.DeactivateEnemies();
        }
        else
        {
            subLevelA.DeactivateEnemies();
        }
    }
}
