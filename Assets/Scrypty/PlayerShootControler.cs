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
        if(Input.GetMouseButtonDown(0))
        {
            weapon.Shoot(cameraOrientation.position, cameraOrientation.forward);
        }
    }
}
