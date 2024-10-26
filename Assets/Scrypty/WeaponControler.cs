using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControler : MonoBehaviour
{
    public float fireRate;
    public int damage;
    public float range;
    public bool isAutomatic;
    
    float nextTimeToFire = 0f;
    
    private Vector3 origin;
    private Vector3 direction;
    
    void Update()
    {
        nextTimeToFire -= Time.deltaTime;
    } 
    
    public bool Shoot(Vector3 origin, Vector3 direction)
    {
        RaycastHit hit;
        if (nextTimeToFire >= 0)
        {
            return false;
        }
        
        if(Physics.Raycast(origin, direction, out hit, range))
        {
            this.direction = direction;
            this.origin = origin;
            //Debug.Log("Hit: " + hit.transform.name);
            BulletHitControler hitControler = hit.transform.GetComponent<BulletHitControler>();
            if(hitControler != null)
            {
                hitControler.Hit(hit, damage);
            }
        }
        
        nextTimeToFire = 1f / fireRate;
        
        return true;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        
        Gizmos.DrawRay(origin, direction * 10f);
    }
}
