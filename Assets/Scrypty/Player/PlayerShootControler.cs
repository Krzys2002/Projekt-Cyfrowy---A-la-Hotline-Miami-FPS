using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerControler))]
public class PlayerShootControler : MonoBehaviour
{
    // Weapon reference
    public WeaponControler weapon;
    // Camera orientation reference
    private Transform cameraOrientation;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get camera orientation
        cameraOrientation = GetComponent<PlayerControler>().cameraOrientation;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if weapon is automatic
        if(weapon.isAutomatic)
        {
            // Check if player is holding mouse button
            if(Input.GetMouseButton(0))
            {
                //Debug.Log("Player is holding mouse button");
                weapon.Shoot(cameraOrientation.position, cameraOrientation.forward);
            }
        }
        else
        {
            // Check if player pressed mouse button
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Player pressed mouse button");
                weapon.Shoot(cameraOrientation.position, cameraOrientation.forward);
            }
        }
    }
}
