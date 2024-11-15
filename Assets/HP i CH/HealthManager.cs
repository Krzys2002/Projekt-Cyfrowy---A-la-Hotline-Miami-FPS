using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image HealthBar;
    public float HealthAmount = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float damage)
    {
        HealthAmount -= damage;
        HealthBar.fillAmount = HealthAmount / 100f;
    }
    public void Heal(float HealingAmount)
    {
        HealthAmount += HealingAmount;
        HealthAmount = Mathf.Clamp(HealthAmount, 0 , 100);

        HealthBar.fillAmount = HealingAmount / 100f;
    }
}
