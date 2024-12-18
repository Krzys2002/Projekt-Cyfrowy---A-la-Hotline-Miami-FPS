using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[DefaultExecutionOrder(0)]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyControler : MonoBehaviour
{
    // Enemy weapon
    public WeaponControler weaponControler;
    // Enemy max hp
    public int maxHp;
    // Enemy accuracy
    public float accuracy = 1f;
    
    // Enemy rotation speed
    static float rotationSpeed = 150f;
    
    // Enemy vision
    [SerializeField]
    public int numberOfRays = 10;
    [SerializeField]
    private GameObject obstacle;
    [SerializeField]
    private bool isStatic = false;
    // Player transform
    private Transform playerTransform;
    // Enemy rays
    private Ray[] rays = new Ray[5];
    // Enemy target rotation
    private Quaternion targetRotation;
    // Is enemy looking at player
    private bool isLookingAtPlayer = false;
    // Random generator
    private System.Random random;
    // Level register array
    private string[] levelRegisterArray;
    // in sublevel
    private SubLevel inSubLevel;
    
    // NavMeshAgent component
    private NavMeshAgent agent;
    
    // Enemy health
    private int currentHp;
    
    private bool canMove = false;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Initialize level register array
        levelRegisterArray = new string[1];
        // Set first element to null
        levelRegisterArray[0] = "null";
        // Get NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get player transform
        playerTransform = GameObject.FindWithTag("Player").transform;
        
        // Initialize random generator
        random = new System.Random();
        // check if player transform is null
        if(playerTransform == null)
        {
            // Log error
            Debug.LogError("Player not found.");
            targetRotation = Quaternion.identity;
        }
        else
        {
            // Set target rotation to look at player
            targetRotation = Quaternion.LookRotation(playerTransform.position - transform.position);
        }
        
        // Set current hp to max hp
        currentHp = maxHp;
    }
    
    // OnEnable is called when the object becomes enabled and active
    private void OnEnable()
    {
        // Get player transform
        playerTransform = GameObject.FindWithTag("Player").transform;
        // start looking at player coroutine
        StartCoroutine(LookAtPlayerCoroutine());
        if (!isStatic)
        {
            StartCoroutine(FollowPlayer());
        }
    }
    
    // OnDisable is called when the behaviour becomes disabled
    private void OnDisable()
    {
        // Stop looking at player coroutine
        StopAllCoroutines();
    }
    
    public bool getIsStatic()
    {
        return isStatic;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate enemy towards player
        float angle = Quaternion.Angle(transform.rotation, targetRotation);
        // Check if angle is greater than 90
        float currentRotationSpeed = angle > 90f ? rotationSpeed * 3f : rotationSpeed;
        // Rotate enemy
        transform.rotation =
            Quaternion.RotateTowards(transform.rotation, targetRotation, currentRotationSpeed * Time.deltaTime);

        if (isLookingAtPlayer && weaponControler)
        {
            // create random deviation from the direction
            Vector3 direction = transform.forward +
                                (Quaternion.Euler(random.Next(-6, 6) * (1f - accuracy),
                                    random.Next(-45, 45) * (1f - accuracy), 0) * transform.forward);
            // Shoot
            WeaponControler.WeaponState weaponStatus = weaponControler.Shoot(transform.position, direction);
            if (weaponStatus == WeaponControler.WeaponState.OutOfAmmo)
            {
                weaponControler.Reload();
            }
            
            // triger enemies in sublevel
            if (!inSubLevel.getTriger())
            {
                inSubLevel.setTriger(true);
            }
        }
    }
    
    // Method to register levels
    public void LevelRegister(string[] levels, SubLevel subLevel)
    {
        levelRegisterArray = levels;
        inSubLevel = subLevel;
    }
    
    // Coroutine to look at player
    private IEnumerator LookAtPlayerCoroutine()
    {
        yield return new WaitForEndOfFrame();
        rays = new Ray[numberOfRays];
        
        while (enabled)
        {
            if (playerTransform == null)
            {
                //Debug.LogError("Player not found.");
                // exit coroutine
                yield break;
            }
            
            // Set target rotation to look at player
            targetRotation = Quaternion.LookRotation(playerTransform.position - transform.position);
            
            // calculate distance to player
            float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
            
            // calculate rays angle spread
            float angleSpread = Math.Clamp(10f/distanceToPlayer, 0.05f, 4f);
                
            // Perform a raycast to check if the enemy is looking directly at the player
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            rays[0] = new Ray(transform.position, directionToPlayer);
            
            float angle = 0f;
            for (int i = 1; i < numberOfRays; i++)
            {
                if (i % 2 == 1)
                {
                    angle = Math.Abs(angle) + angleSpread;
                }
                else
                {
                    angle *= -1;
                }
                
                rays[i] = new Ray(transform.position, Quaternion.Euler(0, angle, 0) * directionToPlayer);
            }
            
            bool playerFound = false;
            bool enemyFound = false;
            
            foreach (Ray ray in rays)
            {
                RaycastHit hit;
                // Check if the ray hit something
                if (!Physics.Raycast(ray, out hit))
                {
                    continue;
                }

                if (hit.transform.CompareTag("Player"))
                { 
                    if(EventManager.Enemies.OnEnemyTriggerByPlayer != null && !canMove)
                    {
                        EventManager.Enemies.OnEnemyTriggerByPlayer.Invoke(this);
                    }
                    // Set target rotation to look at player
                    targetRotation = Quaternion.LookRotation(ray.direction);
                    playerFound = true;
                    canMove = true;
                }

                if (hit.transform.CompareTag("Enemy"))
                {
                    enemyFound = true;
                }
            }
            
            if(playerFound && !enemyFound)
            {
                isLookingAtPlayer = true;
            }
            else
            {
                isLookingAtPlayer = false;
            }
            
            yield return new WaitForSeconds(0.1f); // 10 times per second
        }
    }
    
    private IEnumerator FollowPlayer()
    {
        yield return new WaitForSeconds(0.1f);
        //Debug.Log("Following player.");
        
        while (enabled)
        {
            if (!isLookingAtPlayer && canMove)
            {
                //Debug.Log("Following player.");
                if (StoreData.EnemyData.useObtacie)
                {
                    obstacle.SetActive(false);
                    agent.enabled = true;
                }
                agent.avoidancePriority = 50;
                agent.SetDestination(playerTransform.position);
            }
            else if (Vector3.Distance(transform.position, playerTransform.position) < 10f)
            {
                if(StoreData.EnemyData.useObtacie)
                {
                    agent.enabled = false;
                    obstacle.SetActive(true);
                }
                else
                {
                    agent.avoidancePriority = 1;
                    agent.SetDestination(transform.position);
                }
            }
            else
            {
                yield return new WaitForSeconds(0.5f); // Add 0.5 second delay
                if(StoreData.EnemyData.useObtacie)
                {
                    agent.enabled = false;
                    obstacle.SetActive(true);
                }
                else
                {
                    agent.avoidancePriority = 1;
                    agent.SetDestination(transform.position);
                }
            }
                        
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float rayLength = 10f; // Specify the length of the rays

        if (rays != null)
        {
            foreach (Ray ray in rays)
            {
                Gizmos.DrawRay(ray.origin, ray.direction * rayLength);
            }
        }
    }
    
    public void TakeDamage(RaycastHit hit, int damage)
    {
        //Debug.Log("Enemy hit by: " + hit.transform.name + " for " + damage + " damage.");
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        // Handle enemy death (e.g., play animation, drop loot)
        //Debug.Log("Enemy has died."); 
        if (EventManager.Enemies.OnAnyEnemyDeath != null)
        {
            EventManager.Enemies.OnAnyEnemyDeath.Invoke(this);
        }
        
        foreach (string levelName in levelRegisterArray)
        {
            EventManager.Enemies.OnEnemyDeathFilter(levelName).Invoke(this);
        }
        this.GameObject().SetActive(false);
    }
    
    
    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
    
    public bool CanMove()
    {
        return canMove;
    }

    public SubLevel InSubLevel()
    {
        return inSubLevel;
    }
    
}
