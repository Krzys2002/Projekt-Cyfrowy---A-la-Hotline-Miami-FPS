using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DialogueEditor;

[System.Serializable]
public class Requirement
{
    // Define an enum for the requirement type
    public enum RequirementType
    {
        Interaction,
        LevelPass,
        EnemyDeathCount,
        DialogueWasInteracted
    }

    // Define the requirement type, count or index, and target
    public RequirementType type;
    public int countOrIndex;
    public GameObject target;

    // Define a bool for the completion status and an int for the count
    private bool isCompleted;
    private int count;
    
    // Define a delegate and an event
    public delegate void RequirementCompletedHandler();
    public event RequirementCompletedHandler OnCompleted;

    // Define a constructor that takes a requirement type, count or index, and target
    public Requirement(RequirementType type, int countOrIndex, GameObject target)
    {
        this.type = type;
        this.countOrIndex = countOrIndex;
        this.target = target;
        isCompleted = false;
        
        OnStart();
    }
    
    public void OnStart()
    {
        // Check the requirement type
        switch (type)
        {
            // If the requirement type is interaction
            case RequirementType.Interaction:
                // Add a listener to the OnObjectInteract event
                EventManager.Objects.OnObjectInteract += Check;
                break;
            // If the requirement type is level pass
            case RequirementType.LevelPass:
                // Add a listener to the OnFinishLevel event
                EventManager.Levels.OnFinishLevelFilter(target.name).AddListener(Check);
                break;
            // If the requirement type is enemy death count
            case RequirementType.EnemyDeathCount:
                // Check if the target is not null
                if (target != null)
                {
                    // Add a listener to the OnEnemyDeathFilter event
                    EventManager.Enemies.OnEnemyDeathFilter(target.name).AddListener(Check);
                    
                    // Check if the count or index is 0
                    if(countOrIndex == 0)
                    {
                        // Check if the target is a sublevel
                        if(target.tag == "SubLevel")
                        {
                            countOrIndex = target.GetComponent<SubLevel>().getEnemies().Count;
                        }
                        // Check if the target is a level
                        else if(target.tag == "Level")
                        {
                            countOrIndex = target.GetComponent<LevelControler>().getEnemies().Count;
                        }
                    }
                }
                else
                {
                    // Add a listener to the OnAnyEnemyDeath event
                    EventManager.Enemies.OnAnyEnemyDeath += Check;
                }
                break;
            case RequirementType.DialogueWasInteracted:
                if(StoreData.PlayerData.PreviousConversations.Contains(target.GetComponent<NPCConversation>()))
                {
                    isCompleted = true;
                    OnCompleted.Invoke();
                }
                else
                {
                    EventManager.Player.OnPlayerEnterDialogue += Check;
                }
                break;
                
        }
    }

    public bool IsCompleted()
    {
        return isCompleted;
    }
    
    // Define a method to check the requirement for dialogue interaction
    private void Check(NPCConversation conversation, Transform transform)
    {
        if (conversation == target.GetComponent<NPCConversation>())
        {
            isCompleted = true;
            OnCompleted.Invoke();
        }
    }

    
    // Define a method to check the requirement for enemy death count
    private void Check(Component com)
    {
        count++;
        //Debug.Log("Count: " + count + " CountOrIndex: " + countOrIndex);

        if (countOrIndex <= count)
        {
            isCompleted = true;
            OnCompleted.Invoke();
        }
    }
    
    // Define a method to check the requirement for interaction

    private void Check(GameObject target, int id)
    {
        if (target == this.target && id == countOrIndex)
        {
            isCompleted = true;
            OnCompleted.Invoke();
        }
    }
    
    // Define a method to check the requirement for level pass
    private void Check(GameObject target)
    {
        if (target == this.target)
        {
            isCompleted = true;
            OnCompleted.Invoke();
        }
    }
}