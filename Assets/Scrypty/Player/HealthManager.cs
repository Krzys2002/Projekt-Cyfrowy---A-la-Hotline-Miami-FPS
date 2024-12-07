using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(100)]
public class HealthManager : MonoBehaviour
{
    public Q_Vignette_Single HealthIndicator;

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
        if(HealthIndicator == null)
        {
            Debug.LogError("HealthBar is null");
            Destroy(gameObject);
            return;
        }

        Debug.Log("Health change: " + currentPresent);
        if (currentPresent == 1)
        {
            HealthIndicator.SetVignetteMainScale(0);
        }
        else
        {
            HealthIndicator.mainScale = Mathf.Lerp(0.5f, 6f, 1 - currentPresent);
        }
    }

    private void OnDestroy()
    {
        EventManager.Player.OnPlayerHealthChange -= healthChange;
    }
}
