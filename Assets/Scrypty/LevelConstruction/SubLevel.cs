using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class SubLevel : MonoBehaviour
{
    public List<SubLevel> subLevels;

    List<GameObject> enemies;
    List<Gate> gates;
    
    [SerializeField]
    GameObject enemiesObject;

    private string[] levelRegisterArray;

    private bool playerWasInside = false;

    private bool triger = false;
    
    public Transform respawnPoint;
    private bool wasCleared = false;


    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<GameObject>();
        gates = new List<Gate>();
        levelRegisterArray = new string[1];
        levelRegisterArray[0] = gameObject.name;
        EnemyControler[] childrenEnemy = gameObject.GetComponentsInChildren<EnemyControler>();

        foreach (EnemyControler child in childrenEnemy)
        {
            enemies.Add(child.gameObject);
        }
        
        foreach(SubLevel subLevel in subLevels)
        {
            enemies.AddRange(subLevel.getEnemies());
        }

        wasCleared = false;
        foreach (var levelName in StoreData.LevelData.ClearedLevels)
        {
            if(levelName == gameObject.name)
            {
                wasCleared = true;
                break;
            }
        }
        Debug.Log("wasCleared: " + wasCleared + " " + gameObject.name);
        
        //Debug.Log("SubLevel " + gameObject.name + " has " + enemies.Count + " enemies.");
    }
    
    
    public List<GameObject> getEnemies()
    {
        return enemies;
    }
    
    public void setTriger(bool triger)
    {
        if(wasCleared)
        {
            return;
        }
        
        if (triger)
        {
            //Debug.Log("SubLevel " + gameObject.name + " was triggered.");
            EventManager.Levels.OnSubLevelTrigger.Invoke(this);
            this.triger = triger;
        }
        else
        {
            //Debug.Log("SubLevel " + gameObject.name + " was untriggered.");
            this.triger = triger;
        }
    }
    
    public bool getTriger()
    {
        return triger;
    }
    
    public void playerEnter(Gate enteredGate)
    {
        playerWasInside = true;
        foreach (Gate gate in gates)
        {
            if(gate != enteredGate)
            {
                gate.activateCloseSublevels(this);
            }
        }
    }

    
    public void playerExit(Gate exitedGate)
    {
        if(wasCleared)
        {
            return;
        }
        
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
        if(wasCleared)
        {
            return;
        }
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
            EnemyControler controler = enemy.GetComponent<EnemyControler>();
            if(controler != null)
            {
                controler.LevelRegister(levelRegisterArray, this);
            }
        }

        if (!areEnemiesActive)
        {
            DeactivateEnemies();
        }
    }
    
    public void ClearLevel()
    {
        wasCleared = true;
        //Debug.Log("Clearing level " + gameObject.name);
        StoreData.LevelData.ClearedLevels.Add(gameObject.name);
        StoreData.LevelData.RespawnPoint = respawnPoint.position;
        StoreData.LevelData.LastClearedLevel = gameObject.name;
        Debug.Log("respawnPoint: " + respawnPoint.position);
        foreach (SubLevel subLevel in subLevels)
        {
            subLevel.ClearLevel();
        }
    }
}
