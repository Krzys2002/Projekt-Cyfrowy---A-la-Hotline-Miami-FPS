using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    
    // Enemy health
    private int currentHp;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Initialize level register array
        levelRegisterArray = new string[1];
        // Set first element to null
        levelRegisterArray[0] = "null";
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
    }
    
    // OnEnable is called when the object becomes enabled and active
    private void OnEnable()
    {
        // start looking at player coroutine
        StartCoroutine(LookAtPlayerCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
        float angle = Quaternion.Angle(transform.rotation, targetRotation);
        float currentRotationSpeed = angle > 90f ? rotationSpeed * 3f : rotationSpeed;
        transform.rotation =
            Quaternion.RotateTowards(transform.rotation, targetRotation, currentRotationSpeed * Time.deltaTime);

        if (isLookingAtPlayer)
        {
            //Debug.Log("Enemy is looking directly at the player.");
            Vector3 direction = transform.forward +
                                (Quaternion.Euler(random.Next(-6, 6) * (1.1f - accuracy),
                                    random.Next(-45, 45) * (1.1f - accuracy), 0) * transform.forward);
            weaponControler.Shoot(transform.position, direction);
            
        }
    }

    public void LevelRegister(string[] levels)
    {
        levelRegisterArray = levels;
    }

    private IEnumerator LookAtPlayerCoroutine()
    {
        while (true)
        {
            if (playerTransform != null)
            {
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
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.CompareTag("Player"))
                        {
                            //Debug.Log("Enemy is looking directly at the player.");
                            // Additional actions can be performed here
                            targetRotation = Quaternion.LookRotation(ray.direction);
                            isLookingAtPlayer = true;
                            break;
                        }

                        if(ray.direction == rays[rays.Length - 1].direction)
                        {
                            //Debug.Log("Enemy is not looking at the player.");
                            isLookingAtPlayer = false;
                        }
                    } 
                    
                }
            }
            
            
            
            yield return new WaitForSeconds(0.1f); // 10 times per second
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
    
    
}
