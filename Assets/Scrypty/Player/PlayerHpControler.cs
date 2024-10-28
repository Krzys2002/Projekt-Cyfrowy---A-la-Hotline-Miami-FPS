using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHpControler : MonoBehaviour
{ 
    public int maxHp;
    public float timeFromLastHitToRegen;
    public int regenHpPerSecond;
    
    private int currentHp;
    private float timeFromLastHit;
    float acumulatedHp = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
        timeFromLastHit = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeFromLastHit += Time.deltaTime;

        if (timeFromLastHit >= timeFromLastHitToRegen && currentHp < maxHp)
        {
            RegenerateHealth();
        }
    }
    
    public void TakeDamage(RaycastHit hit, int damage)
    {
        Debug.LogWarning("Player hit by: " + hit.transform.name + " for " + damage + " damage.");
        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        timeFromLastHit = 0f;

        if (currentHp <= 0)
        {
            Die();
        }
    }
    
    private void RegenerateHealth()
    {
        acumulatedHp += regenHpPerSecond * Time.deltaTime;
        if(acumulatedHp >= 1)
        {
            currentHp += (int)acumulatedHp;
            acumulatedHp -= (int)acumulatedHp;
        }
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        
        if(currentHp == maxHp)
        {
            acumulatedHp = 0;
        }
    }
    
    private void Die()
    {
        // Handle player death (e.g., respawn, game over)
        Debug.Log("Player has died.");
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
