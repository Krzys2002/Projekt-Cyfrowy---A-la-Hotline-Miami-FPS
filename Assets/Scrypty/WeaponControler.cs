using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControler : MonoBehaviour
{
    // Weapon fire rate (bullets per second)
    public float fireRate;
    // Weapon damage
    public int damage;
    // Weapon range
    public float range;
    // Weapon type
    public bool isAutomatic;
    
    // Time to next fire
    float nextTimeToFire = 0f;
    
    private Vector3 origin;
    private Vector3 direction;
    

    void Update()
    {
        // Decrease time to next fire
        nextTimeToFire -= Time.deltaTime;
    } 
    
    // Method to shoot
    public bool Shoot(Vector3 origin, Vector3 direction)
    {
        
        RaycastHit hit;
        // Check if time to next fire is greater than 0
        if (nextTimeToFire >= 0)
        {
            return false;
        }
        
        //Debug.Log(layer_mask);
        
        // Check if raycast hit something
        if(Physics.Raycast(origin, direction, out hit, range))
        {
            // Set origin and direction
            this.direction = direction;
            this.origin = origin;
            
            // Check if hit object has BulletHitControler component
            BulletHitControler hitControler = hit.transform.GetComponent<BulletHitControler>();
            if(hitControler != null)
            {
                // Call Hit method on hit object
                hitControler.Hit(hit, damage);
            }
        }
        
        // Set time to next fire
        nextTimeToFire = 1f / fireRate;
        
        return true;
    }
    
    private void OnDrawGizmos()
    {
        // Draw ray
        Gizmos.color = Color.blue;
        
        Gizmos.DrawRay(origin, direction * 10f);
    }
}
