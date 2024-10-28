using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField]
    private BoxCollider colliderA;
    [SerializeField]
    private BoxCollider colliderB;
    
    [SerializeField]
    public SubLevel subLevelA;
    [SerializeField]
    public SubLevel subLevelB;
    
    bool playerInA = false;
    bool playerInB = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start gate");
        subLevelA.addGate(this);
        subLevelB.addGate(this);
    }
    
    public void playerEnterA()
    {
        playerInA = true;
        //Debug.LogWarning("Player enter A");
    }
    
    public void playerExitA()
    {
        playerInA = false;
        //Debug.LogWarning("Player exit A");
        
        if(!playerInB)
        {
            Debug.LogWarning("Player enter " + subLevelA.name);
            subLevelA.playerEnter(this);
            subLevelB.playerExit(this);
        }
    }
    
    public void playerEnterB()
    {
        playerInB = true;
        //Debug.LogWarning("Player enter B");
    }
    
    public void playerExitB()
    {
        playerInB = false;
        
        if(!playerInA)
        {
            Debug.LogWarning("Player enter " + subLevelB.name);
            subLevelB.playerEnter(this);
            subLevelA.playerExit(this);
        }
    }
    
    public void activateClouseSublevels(SubLevel subLevel)
    {
        if(subLevelA == subLevel)
        {
            subLevelB.ActivateEnemies();
        }
        else
        {
            subLevelA.ActivateEnemies();
        }
    }
    
    public void deactivateClouseSublevels(SubLevel subLevel)
    {
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
