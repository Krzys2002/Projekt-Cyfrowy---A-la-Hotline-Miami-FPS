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
        InputMenager.inputMenager.reloadAction += PlayerReload;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 0)
        {
            return;
        }
        // Check if weapon is automatic
        if(weapon.isAutomatic)
        {
            // Check if player is holding mouse button
            if(Input.GetMouseButton(0))
            {
                //Debug.Log("Player is holding mouse button");
                
                WeaponControler.WeaponState state = weapon.GetWeaponState();
                if (state == WeaponControler.WeaponState.OutOfAmmo)
                {
                    PlayerReload();
                }
                
                if(state == WeaponControler.WeaponState.Ready)
                {
                    weapon.Shoot(cameraOrientation.position, cameraOrientation.forward);
                    PlayerAnimation.PlayerShoot();
                }
            }
        }
        else
        {
            // Check if player pressed mouse button
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Player pressed mouse button");
                WeaponControler.WeaponState state = weapon.GetWeaponState();
                if (state == WeaponControler.WeaponState.OutOfAmmo)
                {
                    PlayerReload();
                }
                
                if(state == WeaponControler.WeaponState.Ready)
                {
                    weapon.Shoot(cameraOrientation.position, cameraOrientation.forward);
                    PlayerAnimation.PlayerShoot();
                }
            }
        }
    }
    
    void PlayerReload()
    {
        if(weapon.GetWeaponState() == WeaponControler.WeaponState.Reloading)
        {
            return;
        }
        PlayerAnimation.PlayerReload();
        weapon.Reload();
    }
}
