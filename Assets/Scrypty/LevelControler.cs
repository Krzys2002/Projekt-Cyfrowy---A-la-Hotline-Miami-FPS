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
        foreach(SubLevel subLevel in subLevels)
        {
            enemies.AddRange(subLevel.getEnemies());
        }
        
        startSubLevel.ActivateEnemies();
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
