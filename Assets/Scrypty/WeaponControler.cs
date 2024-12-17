using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class WeaponControler : MonoBehaviour
{
    public enum WeaponState
    {
        Ready,
        OnCooldown,
        OutOfAmmo,
        Reloading
    }

    
    // Weapon fire rate (bullets per second)
    public float fireRate;
    // Weapon damage
    public int damage;
    // Weapon range
    public float range;
    // Max ammo
    public int maxAmmo;
    // reload time
    public float reloadTime;
    // Weapon type
    public bool isAutomatic;
    
    public UnityAction<int> OnAmmoChange;
    
    public AudioSource audioSource; // Audio source for shooting sound
    public AudioClip shootingSound; // Shooting sound
    public AudioClip reloadingSound; // Reloading sound
    
    // light for shooting
    public Light shootingLight;
    public float lightDuration;
    
    private bool lightEnabled;
    private float lightTime;
    
    // Time to next fire
    float nextTimeToFire = 0f;
    
    private Vector3 origin;
    private Vector3 direction;
    
    
    // Current ammo
    // add geter and seter
    private int _currentAmmo;
    public int currentAmmo
    {
        get => _currentAmmo;
        set
        {
            _currentAmmo = value;
            OnAmmoChange?.Invoke(_currentAmmo);
        }
    }
    // State of the weapon
    private WeaponState weaponState;
    
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = maxAmmo;
        weaponState = WeaponState.Ready;
    }
    

    void Update()
    {
        // Decrease time to next fire
        nextTimeToFire -= Time.deltaTime;
        
        UpdateLight();
        UpdateState();
    } 
    
    // Method to Get weapon state
    public WeaponState GetWeaponState()
    {
        return weaponState;
    }
    
    // Method to shoot
    public WeaponState Shoot(Vector3 origin, Vector3 direction)
    {
        RaycastHit hit;
        // Check if weapon is ready to shoot
        if(weaponState != WeaponState.Ready)
        {
            return weaponState;
        }
        
        currentAmmo--;
        
        // Set time to next fire
        nextTimeToFire = 1f / fireRate;
        
        // Play shooting sound
        audioSource.PlayOneShot(shootingSound);
        TriggerShootLight();
        
        weaponState = WeaponState.OnCooldown;
        
        if(currentAmmo <= 0)
        {
            weaponState = WeaponState.OutOfAmmo;
        }
        
        // Set origin and direction
        this.direction = direction;
        this.origin = origin;
        
        // Check if raycast hit something
        if(!Physics.Raycast(origin, direction, out hit, range))
        {
            return weaponState;
        }
            
        // Check if hit object has BulletHitControler component
        BulletHitControler hitControler = hit.transform.GetComponent<BulletHitControler>();
        if(hitControler != null)
        {
            // Call Hit method on hit object
            hitControler.Hit(hit, damage);
        }
        
        return weaponState;
    }
    
    public bool isEmpty()
    {
        return currentAmmo <= 0;
    }
    
    public void Reload()
    {
        weaponState = WeaponState.Reloading;
        nextTimeToFire = reloadTime;
        // Play reloading sound
        audioSource.PlayOneShot(reloadingSound);
    }
    
    public int GetAmountOfAmmo()
    {
        return currentAmmo;
    }
    
    private void TriggerShootLight()
    {
        shootingLight.gameObject.SetActive(true);
        lightEnabled = true;
        lightTime = lightDuration;
    }
    
    private void UpdateLight()
    {
        if(lightEnabled)
        {
            lightTime -= Time.deltaTime;
            if(lightTime <= 0)
            {
                shootingLight.gameObject.SetActive(false);
                lightEnabled = false;
            }
        }
    }
    
    private void UpdateState()
    {
        if(nextTimeToFire >= 0 && weaponState != WeaponState.Reloading && weaponState != WeaponState.OutOfAmmo)
        {
            weaponState = WeaponState.OnCooldown;
        }
        
        if(nextTimeToFire < 0 && weaponState != WeaponState.OutOfAmmo)
        {
            if(weaponState == WeaponState.Reloading)
            {
                currentAmmo = maxAmmo;
            }
            weaponState = WeaponState.Ready;
        }
    }
    
    private void OnDrawGizmos()
    {
        // Draw ray
        Gizmos.color = Color.blue;
        
        Gizmos.DrawRay(origin, direction * 10f);
    }
}
