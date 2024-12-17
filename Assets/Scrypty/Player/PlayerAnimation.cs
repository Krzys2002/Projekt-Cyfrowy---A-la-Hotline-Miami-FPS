using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(15)]
public class PlayerAnimation : MonoBehaviour
{
    public static UnityAction PlayerShoot;
    public static UnityAction PlayerReload;
    public static UnityAction PlayerStartMoving;
    public static UnityAction PlayerStopMoving;
    
    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerShoot += Shoot;
        PlayerReload += Reload;
        PlayerStartMoving += StartMoving;
        PlayerStopMoving += StopMoving;
    }
    
    // triger shoot animation
    void Shoot()
    {
        animator.SetTrigger("ShootTrigger");
    }
    
    // triger reload animation
    void Reload()
    {
        animator.SetTrigger("ReloadTrigger");
    }
    
    // change moving animation
    void StartMoving()
    {
        animator.SetBool("IsMoving", true);
    }
    
    // change moving animation
    void StopMoving()
    {
        animator.SetBool("IsMoving", false);
    }
}
