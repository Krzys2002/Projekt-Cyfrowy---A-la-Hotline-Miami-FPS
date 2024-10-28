using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControler : MonoBehaviour
{
    public RequirementsManager requirementsManager;
    
    public SubLevel startSubLevel;
    
    public List<SubLevel> subLevels;
    
    List<GameObject> enemies;
    
    
    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<GameObject>();
        
        foreach(SubLevel subLevel in subLevels)
        {
            enemies.AddRange(subLevel.getEnemies());
        }
        
        string[] levelReg = new string[1];
        levelReg[0] = gameObject.name;
        
        foreach (SubLevel subLevel in subLevels)
        {
            Debug.Log("register " + levelReg[0]);
            subLevel.LevelsRegisters(levelReg);
            subLevel.DeactivateEnemies();
        }


        StartCoroutine(StartGame());
    }
    
    IEnumerator StartGame()
    {
        // Wait for the end of the frame to ensure Start has completed
        yield return new WaitForEndOfFrame();
        
        startSubLevel.ActivateEnemies();
        Debug.Log("Player enter");
        startSubLevel.playerEnter(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public List<GameObject> getEnemies()
    {
        return enemies;
    }
}
