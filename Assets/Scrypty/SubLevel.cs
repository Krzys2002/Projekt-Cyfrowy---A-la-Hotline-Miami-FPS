using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubLevel : MonoBehaviour
{
    public List<SubLevel> subLevels;

    List<GameObject> enemies;
    List<Gate> gates;
    
    GameObject enemiesObject;

    private void Awake()
    {
        enemies = new List<GameObject>();
        gates = new List<Gate>();
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            //Debug.Log("Child name: " + child.name);
            if (child.name == "Enemies")
            {
                Transform[] enemiesTransforms = child.GetComponentsInChildren<Transform>();
                
                Debug.Log("Enemies count: " + enemiesTransforms.Length);
                
                enemiesObject = child.gameObject;

                foreach (Transform enemyTransform in enemiesTransforms)
                {
                    if (enemyTransform.gameObject.tag == "Enemy")
                    {
                        enemies.Add(enemyTransform.gameObject);
                    }
                }
                
                break;
            }
        }
    }
    


    // Start is called before the first frame update
    void Start()
    {
        foreach(SubLevel subLevel in subLevels)
        {
            enemies.AddRange(subLevel.getEnemies());
        }
        
        Debug.Log("SubLevel " + gameObject.name + " has " + enemies.Count + " enemies.");
        
        DeactivateEnemies();
    }
    
    
    public List<GameObject> getEnemies()
    {
        return enemies;
    }
    
    public void playerEnter(Gate enteredGate)
    {
        foreach (Gate gate in gates)
        {
            if(gate != enteredGate)
            {
                Debug.Log("Gate " + gate.gameObject.name + " is closing sublevels.");
                gate.activateClouseSublevels(this);
            }
        }
    }

    
    public void playerExit(Gate exitedGate)
    {
        foreach (Gate gate in gates)
        {
            if(gate != exitedGate)
            {
                gate.deactivateClouseSublevels(this);
            }
        }
    }
    
    public void addGate(Gate gate)
    {
        gates.Add(gate);
    }

    
    public void ActivateEnemies()
    {
        Debug.Log("Activating enemies in " + gameObject.name);
        enemiesObject.SetActive(true);
    }
    
    public void DeactivateEnemies()
    {
        Debug.Log("Deactivating enemies in " + gameObject.name);
        Debug.Log(enemiesObject);
        enemiesObject.SetActive(false);
    }
}
