using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControler : MonoBehaviour
{
    public float fireRate;
    public float damage;
    public float range;
    public GameObject prefab;
    
    public RaycastHit Shoot(Vector3 origin, Vector3 direction)
    {
        RaycastHit hit;
        
        if(Physics.Raycast(origin, direction, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);
            Instantiate(prefab, hit.point, Quaternion.identity);
        }
        
        return hit;
    }
}
