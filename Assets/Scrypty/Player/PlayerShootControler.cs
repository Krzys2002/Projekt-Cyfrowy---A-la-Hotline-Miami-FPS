using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShootControler : MonoBehaviour
{
    public WeaponControler weapon;
    public Transform cameraOrientation;

    // Update is called once per frame
    void Update()
    {
        if(weapon.isAutomatic)
        {
            if(Input.GetMouseButton(0))
            {
                weapon.Shoot(cameraOrientation.position, cameraOrientation.forward);
                //EventManager.Enemies.OnAnyEnemyDeath.Invoke(this);
                //Debug.Log("Test1");
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                weapon.Shoot(cameraOrientation.position, cameraOrientation.forward);
                //EventManager.Enemies.OnAnyEnemyDeath.Invoke(this);
                //Debug.Log("Test1");
            }
        }
    }
}
