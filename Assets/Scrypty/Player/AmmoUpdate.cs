using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class AmmoUpdate : MonoBehaviour
{
    public WeaponControler weaponControler;
    public TextMeshProUGUI ammoCounter;
    // Start is called before the first frame update
    void Start()
    {
        weaponControler.OnAmmoChange += UpdateAmmoCounter;
        ammoCounter.text = weaponControler.GetAmountOfAmmo().ToString();
    }

    void UpdateAmmoCounter(int ammo)
    {
        ammoCounter.text = ammo.ToString();
    }
}
