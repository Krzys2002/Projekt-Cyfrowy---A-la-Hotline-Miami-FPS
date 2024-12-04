using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(100)]
public class HealthManager : MonoBehaviour
{
    public Image HealthBar;

    // Start is called before the first frame update
    void Start()
    {
        //EventManager.Player.OnPlayerHealthChange += healthChange;
    }

    private void OnEnable()
    {
        EventManager.Player.OnPlayerHealthChange += healthChange;
    }

    public void healthChange(float currentPresent)
    {
        if(HealthBar == null)
        {
            Debug.LogError("HealthBar is null");
            Destroy(gameObject);
            return;
        }
        HealthBar.fillAmount = currentPresent;
    }

    private void OnDestroy()
    {
        EventManager.Player.OnPlayerHealthChange -= healthChange;
    }
}
