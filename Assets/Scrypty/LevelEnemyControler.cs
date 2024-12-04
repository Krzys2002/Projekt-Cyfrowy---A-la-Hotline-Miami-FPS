using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(4)]
[RequireComponent(typeof(LevelControler))]
public class LevelEnemyControler : MonoBehaviour
{
    private List<GameObject> enemies;
    
    [SerializeField]
    private int maxNumberOfActiveEnemies = 10;
    
    int currentNumberOfActiveEnemies = 0;
    
    private Transform playerTransform;
    private void Start()
    {
        enemies = new List<GameObject>();
        enemies = GetComponent<LevelControler>().getEnemies();

        for (int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i] == null)
            {
                enemies.RemoveAt(i);
                i--;
            }
        }
        playerTransform = GameObject.FindWithTag("Player").transform;
        EventManager.Levels.OnSubLevelTrigger += onSublevelTrigger;
        activateEnemies();
    }

    private void OnEnable()
    {
        //enemies = new List<GameObject>();
        if(enemies != null)
        {
            enemies = GetComponent<LevelControler>().getEnemies();
        }
        if(enemies != null)
        {
            activateEnemies();
        }
    }

    public void enemyDied(GameObject enemy)
    {
        currentNumberOfActiveEnemies--;
        if (currentNumberOfActiveEnemies <= maxNumberOfActiveEnemies)
        {
            activateEnemies();
        }
    }

    private void onSublevelTrigger(SubLevel s)
    {
        activateEnemies();
    }
    
    private void OnEnemyTrigger(Component enemy)
    {
        currentNumberOfActiveEnemies++;
    }

    private void onEnemyDeath(Component enemy)
    {
        currentNumberOfActiveEnemies--;
        enemies.Remove(enemy.gameObject);
        activateEnemies();
    }
    
    public void activateEnemies()
    {
        List<(GameObject enemy, float distance)> enemyDistances = new List<(GameObject, float)>();
        if(enemies == null)
        {
            Debug.LogError("Enemies is null");
            return;
        }

        //Debug.Log(enemies.Count);
        foreach (GameObject enemy in enemies)
        {
            if(enemy == null)
            {
                continue;
            }
            
            if(!enemy.activeSelf)
            {
                continue;
            }
            float distance = Vector3.Distance(playerTransform.position, enemy.transform.position);
            enemyDistances.Add((enemy, distance));
        }

        enemyDistances.Sort((a, b) => a.distance.CompareTo(b.distance));

        foreach (var (enemy, distance) in enemyDistances)
        {
            if (currentNumberOfActiveEnemies >= maxNumberOfActiveEnemies)
            {
                //Debug.Log("Max number of active enemies reached");
                break;
            }
            
            EnemyControler enemyControler = enemy.GetComponent<EnemyControler>();

            if (enemyControler.InSubLevel().getTriger() == false)
            {
                //Debug.Log("Enemy is not in active sublevel " + enemy.name);
                continue;
            }

            if (enemyControler.CanMove())
            {
                continue;
            }
            
            Debug.Log("Activating enemy: " + enemy.name);
            enemyControler.SetCanMove(true);
            currentNumberOfActiveEnemies++;
            
        }
    }
}
