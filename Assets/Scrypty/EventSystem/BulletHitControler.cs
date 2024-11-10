using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Define a UnityEvent that takes a RaycastHit and an int as parameters
[System.Serializable]
public class HitEvent : UnityEvent<RaycastHit, int> { }

public class BulletHitControler : MonoBehaviour
{
    public HitEvent hitEvent;

    public void Hit(RaycastHit hit, int damage)
    {
        hitEvent.Invoke(hit, damage);
    }
}
