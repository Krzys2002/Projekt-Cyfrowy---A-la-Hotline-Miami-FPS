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
    
    public GameObject animatorObject;
    
    private Animator animator;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        animator = animatorObject.GetComponent<Animator>();
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
    
    void OnDestroy()
    {
        PlayerShoot -= Shoot;
        PlayerReload -= Reload;
        PlayerStartMoving -= StartMoving;
        PlayerStopMoving -= StopMoving;
    }
}
