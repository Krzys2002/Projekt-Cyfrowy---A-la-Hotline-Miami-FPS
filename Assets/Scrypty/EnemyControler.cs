using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    public WeaponControler weaponControler;
    public int maxHp;
    
    
    static float rotationSpeed = 150f;
    
    // Enemy vision
    private Transform playerTransform;
    private Ray[] rays = new Ray[5];
    private Quaternion targetRotation;
    private bool isLookingAtPlayer = false;
    
    // Enemy health
    private int currentHp;
    
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        if(playerTransform == null)
        {
            Debug.LogError("Player not found.");
            targetRotation = Quaternion.identity;
        }
        else
        {
            targetRotation = Quaternion.LookRotation(playerTransform.position - transform.position);
        }
        
        currentHp = maxHp;
        StartCoroutine(LookAtPlayerCoroutine());
    }

    void Update()
    {
        float angle = Quaternion.Angle(transform.rotation, targetRotation);
        float currentRotationSpeed = angle > 90f ? rotationSpeed * 3f : rotationSpeed;
        transform.rotation =
            Quaternion.RotateTowards(transform.rotation, targetRotation, currentRotationSpeed * Time.deltaTime);

        if (isLookingAtPlayer)
        {
            weaponControler.Shoot(transform.position, transform.forward);
        }
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
                    }

                    if(ray.direction == rays[rays.Length - 1].direction)
                    {
                        isLookingAtPlayer = false;
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
        Destroy(gameObject);
    }
}
