using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SubLevel : MonoBehaviour
{
    public List<SubLevel> subLevels;

    List<GameObject> enemies;
    List<Gate> gates;
    
    GameObject enemiesObject;

    private string[] levelRegisterArray;

    private bool playerWasInside = false;

    private void Awake()
    {
        enemies = new List<GameObject>();
        gates = new List<Gate>();
        levelRegisterArray = new string[1];
        levelRegisterArray[0] = gameObject.name;
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            //Debug.Log("Child name: " + child.name);
            if (child.name == "Enemies")
            {
                Transform[] enemiesTransforms = child.GetComponentsInChildren<Transform>();
                
                //Debug.Log("Enemies count: " + enemiesTransforms.Length);
                
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
        
        foreach(SubLevel subLevel in subLevels)
        {
            enemies.AddRange(subLevel.getEnemies());
        }
        
        //Debug.Log("SubLevel " + gameObject.name + " has " + enemies.Count + " enemies.");
    }
    


    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    
    public List<GameObject> getEnemies()
    {
        return enemies;
    }
    
    public void playerEnter(Gate enteredGate)
    {
        //Debug.Log("Player enter " + gameObject.name);
        //Debug.Log("Number of gates: " + gates.Count);
        playerWasInside = true;
        foreach (Gate gate in gates)
        {
            if(gate != enteredGate)
            {
                //Debug.Log("Activate clouse sublevels at gate " + gate.gameObject.name);
                gate.activateCloseSublevels(this);
            }
        }

        enableMove();
    }

    
    public void playerExit(Gate exitedGate)
    {
        foreach (Gate gate in gates)
        {
            if(gate != exitedGate)
            {
                gate.deactivateCloseSublevels(this);
            }
        }
    }
    
    public void addGate(Gate gate)
    {
        gates.Add(gate);
    }

    
    public void ActivateEnemies()
    {
        //Debug.Log("Activating enemies in " + gameObject.name);
        enemiesObject.SetActive(true);
    }
    
    public void DeactivateEnemies()
    {
        //Debug.Log("Deactivating enemies in " + gameObject.name);
        //Debug.Log(enemiesObject.name);
        if (!playerWasInside)
        {
            enemiesObject.SetActive(false);
        }
    }

    public void LevelsRegisters(string[] levels)
    {
        levelRegisterArray = new string[levels.Length + 1];
        levels.CopyTo(levelRegisterArray, 0);
        levelRegisterArray[levelRegisterArray.Length - 1] = gameObject.name;

        foreach (SubLevel subLevel in subLevels)
        {
            subLevel.LevelsRegisters(levelRegisterArray);
        }

        bool areEnemiesActive = enemiesObject.activeSelf;
        ActivateEnemies();
        

        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyControler>().LevelRegister(levelRegisterArray, this);
        }

        if (!areEnemiesActive)
        {
            DeactivateEnemies();
        }
    }

    public void enableMove()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyControler>().SetCanMove(true);
        }
    }
}
