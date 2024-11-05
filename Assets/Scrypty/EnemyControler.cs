using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
        // Start looking at player coroutine
        StartCoroutine(LookAtPlayerCoroutine());
        StartCoroutine(FollowPlayer());
    }
    
    // OnEnable is called when the object becomes enabled and active
    private void OnEnable()
    {
        // Get player transform
        playerTransform = GameObject.FindWithTag("Player").transform;
        // start looking at player coroutine
        StartCoroutine(LookAtPlayerCoroutine());
        StartCoroutine(FollowPlayer());
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
        
        // Check if enemy is looking at player
        if (isLookingAtPlayer)
        {
            // create random deviation from the direction
            Vector3 direction = transform.forward +
                                (Quaternion.Euler(random.Next(-6, 6) * (1.1f - accuracy),
                                    random.Next(-45, 45) * (1.1f - accuracy), 0) * transform.forward);
            // Shoot
            weaponControler.Shoot(transform.position, direction);
        }
    }
    
    // Method to register levels
    public void LevelRegister(string[] levels)
    {
        levelRegisterArray = levels;
    }
    
    // Coroutine to look at player
    private IEnumerator LookAtPlayerCoroutine()
    {
        while (enabled)
        {
            if (playerTransform == null)
            {
                Debug.LogError("Player not found.");
                // exit coroutine
                yield break;
            }
            
            // Set target rotation to look at player
            targetRotation = Quaternion.LookRotation(playerTransform.position - transform.position);
                
            // Perform a raycast to check if the enemy is looking directly at the player
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            rays[0] = new Ray(transform.position, directionToPlayer);
            rays[1] = new Ray(transform.position, Quaternion.Euler(0, 0.4f, 0) * directionToPlayer);
            rays[2] = new Ray(transform.position, Quaternion.Euler(0, -0.4f, 0) * directionToPlayer);
            rays[3] = new Ray(transform.position, Quaternion.Euler(0, 0.8f, 0) * directionToPlayer);
            rays[4] = new Ray(transform.position, Quaternion.Euler(-0, -0.8f, 0) * directionToPlayer);

            foreach (Ray ray in rays)
            {
                RaycastHit hit;
                // Check if the ray hit something
                if (Physics.Raycast(ray, out hit))
                {
                    // Check if the ray is looking at the player
                    if (hit.transform.CompareTag("Player"))
                    {
                        // Set target rotation to look at player
                        targetRotation = Quaternion.LookRotation(ray.direction);
                        isLookingAtPlayer = true;
                        break;
                    }
                        
                    // Check if the last ray is not looking at the player
                    if(ray.direction == rays[rays.Length - 1].direction)
                    {
                        isLookingAtPlayer = false;
                    }
                } 
                    
            }
            
            yield return new WaitForSeconds(0.1f); // 10 times per second
        }
    }
    
    private IEnumerator FollowPlayer()
    {
        yield return new WaitForSeconds(0.1f);
        
        while (enabled)
        {
            if (!isLookingAtPlayer && canMove)
            {
                agent.SetDestination(playerTransform.position);
            }
            else
            {
                agent.SetDestination(transform.position);
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
        Debug.Log("Enemy hit by: " + hit.transform.name + " for " + damage + " damage.");
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        // Handle enemy death (e.g., play animation, drop loot)
        Debug.Log("Enemy has died."); 
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
    
}
