using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(3)]
public class LevelControler : MonoBehaviour
{
    // Requirements manager reference
    public RequirementsManager requirementsManager;
    
    // Start sublevel
    public SubLevel startSubLevel;
    
    // Sublevels in level
    public List<SubLevel> subLevels;
    
    // Enemies in level
    List<GameObject> enemies;
    
    
    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<GameObject>();
        
        // Get all enemies in level
        foreach(SubLevel subLevel in subLevels)
        {
            enemies.AddRange(subLevel.getEnemies());
        }
        
        // Register level in level register
        string[] levelReg = new string[1];
        levelReg[0] = gameObject.name;
        
        // Register level in sublevels
        foreach (SubLevel subLevel in subLevels)
        {
            subLevel.LevelsRegisters(levelReg);
            subLevel.DeactivateEnemies();
        }
        
        
        // Activate start sublevel
        startSubLevel.ActivateEnemies();
        // Player enters start sublevel
        startSubLevel.playerEnter(null);
        startSubLevel.ActivateEnemies();
    }

    // Get enemies in level
    public List<GameObject> getEnemies()
    {
        return enemies;
    }
}
