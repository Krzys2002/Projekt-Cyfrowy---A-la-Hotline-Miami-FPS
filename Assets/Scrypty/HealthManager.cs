using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image HealthBar;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Player.OnPlayerHealthChange += healthChange;
    }
    
    public void healthChange(float currentPresent)
    {
        HealthBar.fillAmount = currentPresent;
    }
}
