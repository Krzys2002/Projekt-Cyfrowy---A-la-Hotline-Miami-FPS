using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShootControler : MonoBehaviour
{
    // Weapon reference
    public WeaponControler weapon;
    // Camera orientation reference
    public Transform cameraOrientation;

    // Update is called once per frame
    void Update()
    {
        // Check if weapon is automatic
        if(weapon.isAutomatic)
        {
            // Check if player is holding mouse button
            if(Input.GetMouseButton(0))
            {
                weapon.Shoot(cameraOrientation.position, cameraOrientation.forward);
            }
        }
        else
        {
            // Check if player pressed mouse button
            if (Input.GetMouseButtonDown(0))
            {
                weapon.Shoot(cameraOrientation.position, cameraOrientation.forward);
            }
        }
    }
}
