using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHpControler : MonoBehaviour
{ 
    // Player max hp
    public int maxHp;
    // Time from last hit to start regenerating hp
    public float timeFromLastHitToRegen;
    // Hp regenerated per second
    public int regenHpPerSecond;
    
    // Current hp
    private int currentHp;
    // Time from last hit
    private float timeFromLastHit;
    // Acumulated hp
    float acumulatedHp = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        // Set current hp to max hp
        currentHp = maxHp;
        
        // Set time from last hit to 0
        timeFromLastHit = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate time from last hit
        timeFromLastHit += Time.deltaTime;

        // Check if player should regenerate hp
        if (timeFromLastHit >= timeFromLastHitToRegen && currentHp < maxHp)
        {
            RegenerateHealth();
        }
    }

    void changeHP(int new_HP)
    {
        currentHp = new_HP;
        
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        
        EventManager.Player.OnPlayerHealthChange.Invoke(currentHp);
        
        // Check if player is dead
        if (currentHp <= 0)
        {
            Die();
        }
    }
    
    // Function to take damage
    public void TakeDamage(RaycastHit hit, int damage)
    {
        int new_HP = currentHp;
        
        // Reduce player hp
        new_HP -= damage;
        // Reset time from last hit
        timeFromLastHit = 0f;
        
        changeHP(new_HP);
    }
    
    // Function to regenerate hp
    private void RegenerateHealth()
    {
        int new_HP = currentHp;
        // add hp per deltatime to acumulated hp
        acumulatedHp += regenHpPerSecond * Time.deltaTime;
        
        // check if acumulated hp is greater than 1
        if(acumulatedHp >= 1)
        {
            // add 1 to current hp
            new_HP += (int)acumulatedHp;
            // remove 1 from acumulated hp
            acumulatedHp -= (int)acumulatedHp;
        }
        
        // Check if player has full hp
        if (new_HP >= maxHp)
        {
            // Reset acumulated hp
            acumulatedHp = 0;
        }
        
        changeHP(new_HP);
    }
    
    // Function to handle player death
    private void Die()
    {
        // Handle player death (e.g., respawn, game over)
        Debug.Log("Player has died.");
        // Restart current scene
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
