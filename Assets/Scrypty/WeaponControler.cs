using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControler : MonoBehaviour
{
    public float fireRate;
    public float damage;
    public float range;
    public bool isAutomatic;
    public GameObject prefab;
    
    float nextTimeToFire = 0f;
    
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
            Debug.Log("Hit: " + hit.transform.name);
            Instantiate(prefab, hit.point, Quaternion.identity);
        }
        
        nextTimeToFire = 1f / fireRate;
        
        return true;
    }
}
