using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

[DefaultExecutionOrder(4)]
[RequireComponent(typeof(LevelControler))]
public class LevelEnemyControler : MonoBehaviour
{
    private List<GameObject> enemies;

    [SerializeField]
    private int maxNumberOfActiveEnemies = 10;
    [SerializeField]
    private float delayBetweenActivationsByDeath = 1f;
    [SerializeField]
    private float delayBetweenActivationsByTrriger = 1f;
    [SerializeField]
    private bool activateEnemiesOnStart = true;

    int currentNumberOfActiveEnemies = 0;
    private bool isActivatingEnemies = false;
    private Transform playerTransform;
    float delay = 0f;
    
    private LevelControler levelControler;

    private void Start()
    {
        levelControler = GetComponent<LevelControler>();
        enemies = new List<GameObject>();
        enemies = levelControler.getEnemies();

        // Remove null enemies
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
        EventManager.Enemies.OnEnemyTriggerByPlayer += OnEnemyTrigger;
        EventManager.Enemies.OnAnyEnemyDeath += onEnemyDeath;
        
        if (activateEnemiesOnStart)
        {
            levelControler.TriggerStartingSubLevel();
            StartCoroutine(ActivateEnemiesWithDelay());
        }
    }
    

    // then some sublevel triggers enemy activation
    private void onSublevelTrigger(SubLevel s)
    {
        delay = delayBetweenActivationsByTrriger;
        StartCoroutine(ActivateEnemiesWithDelay());
    }

    // then player triggers enemy activation
    private void OnEnemyTrigger(Component enemy)
    {
        Debug.Log("Enemy triggered " + enemy.name);
        currentNumberOfActiveEnemies++;
    }

    // then enemy dies
    private void onEnemyDeath(Component enemy)
    {
        
        // remove enemy from list
        enemies.Remove(enemy.gameObject);
        currentNumberOfActiveEnemies--;
        // if there are less enemies than max, activate more
        if (currentNumberOfActiveEnemies <= maxNumberOfActiveEnemies)
        {
            delay = delayBetweenActivationsByDeath;
            StartCoroutine(ActivateEnemiesWithDelay());
        }
    }

    // activate enemies with delay
    private IEnumerator ActivateEnemiesWithDelay()
    {
        // if already activating, return
        if (isActivatingEnemies)
        {
            yield break;
        }

        // set activating to true
        isActivatingEnemies = true;
        yield return new WaitForSeconds(delay); // Adjust the delay as needed
        
        // list of enemies and their distances to player
        List<(GameObject enemy, float distance)> enemyDistances = new List<(GameObject, float)>();
        
        
        if(enemies == null)
        {
            yield break;
        }
        

        // find distance to player for each enemy and check if is active
        foreach (GameObject enemy in enemies)
        {
            if (enemy == null || !enemy.activeSelf)
            {
                continue;
            }
            
            Vector3 position = enemy.transform.position;
            
            NavMeshHit hit;
            if (NavMesh.SamplePosition(position, out hit, 100, NavMesh.AllAreas))
            {
                position = hit.position;
            }
            
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(playerTransform.position, position, NavMesh.AllAreas, path))
            {
                float distance = 0f;
                for (int i = 1; i < path.corners.Length; i++)
                {
                    distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }
                enemyDistances.Add((enemy, distance));
            }
        }
        
        // sort by distance
        enemyDistances.Sort((a, b) => a.distance.CompareTo(b.distance));

        // loop that set enemies that can move from now on
        foreach (var (enemy, distance) in enemyDistances)
        {
            // if there are enough enemies, break
            if (currentNumberOfActiveEnemies >= maxNumberOfActiveEnemies)
            {
                break;
            }

            EnemyControler enemyControler = enemy.GetComponent<EnemyControler>();

            if (enemyControler.getIsStatic())
            {
                continue;
            }
            

            if (enemyControler.InSubLevel().getTriger() == false)
            {
                continue;
            }

            if (enemyControler.CanMove())
            {
                continue;
            }
            
            enemyControler.SetCanMove(true);
            currentNumberOfActiveEnemies++;
        }
        
        // free the lock
        isActivatingEnemies = false;
    }

    private void OnDestroy()
    {
        StopCoroutine(ActivateEnemiesWithDelay());
        EventManager.Levels.OnSubLevelTrigger -= onSublevelTrigger;
        EventManager.Enemies.OnEnemyTriggerByPlayer -= OnEnemyTrigger;
        EventManager.Enemies.OnAnyEnemyDeath -= onEnemyDeath;
    }
}